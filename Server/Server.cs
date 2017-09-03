using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using Contract;
using System.ComponentModel;

namespace Servak
{
    public class Server :INotifyPropertyChanged
    {
        MyChat contract = new MyChat();

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                if (_users == value) return;
                _users = value;
                OnPropertyChanged();
            }
        }

        Dictionary<String, String> files = new Dictionary<String, String>();

        XmlSerializer xmlHandler = new XmlSerializer(typeof(List<User>));

        bool bye_bye = true;

        private relayCommand _closeComm;
        public relayCommand closeComm
        {
            get { return _closeComm; }
            set { _closeComm = value; }
        }

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        TcpListener serv;
        List<Thread> threadList = new List<Thread>();
        Dictionary<TcpClient, User> clientList = new Dictionary<TcpClient, User>();

        public Server()
        {
            serv = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), 10001);

            using (var reader = new StreamReader("users.xml"))
            {
               Users = new ObservableCollection<User>((List<User>)xmlHandler.Deserialize(reader));
            }

            closeComm = new relayCommand(param => disconnectAll());
            serv.Start();
            Thread t = new Thread(listen) { IsBackground = true };
            threadList.Add(t);
            t.Start();
        }

        private void listen()
        {
            TcpClient tcp = new TcpClient();

            while (bye_bye)
            {
                tcp = serv.AcceptTcpClient();
                Thread t = new Thread(() => ListenerClient(tcp)) { IsBackground = true };
                threadList.Add(t);
                t.Start();
            }
        }

        public void disconnectAll()
        {
            serv.Stop();
            bye_bye = false;
            for (int i = threadList.Count - 1; i >= 0; i--)
            {
                threadList[i].Abort();
                threadList[i].Join();
            }

            foreach (var item in clientList)
            {
                clientList[item.Key].isConnected = false;
                item.Key.GetStream().Flush();
                item.Key.GetStream().Close();
                item.Key.Close();
            }
        }

        public void ListenerClient(TcpClient tcp)
        {

            CommandType comand = new CommandType();
            BinaryFormatter bf = new BinaryFormatter();
            BinaryReader br = new BinaryReader(tcp.GetStream());
            BinaryWriter bw = new BinaryWriter(tcp.GetStream());
            User au = new User();

            while (bye_bye)
            {
                comand = (CommandType)br.ReadByte();

                switch (comand)
                {
                    case CommandType.Connect:
                        au = bf.Deserialize(tcp.GetStream()) as User;
                        if (Users.Where(x => x.Login == au.Login).Count() != 0)
                        {
                            Users.First(x => x.Login == au.Login).isConnected = true;
                            clientList.Add(tcp, au);
                            bw.Write(true);
                        }
                        else
                        {
                            bw.Write(false);
                        }
                        break;
                    case CommandType.Disconect:
                        if (au.Login != null) Users.First(x => x.Login == au.Login).isConnected = false;
                        clientList.Remove(tcp);
                        tcp.GetStream().Close();
                        tcp.Close();
                        bye_bye = false;
                        break;

                    case CommandType.checkUser:
                        var user = bf.Deserialize(tcp.GetStream()) as User;

                        if (Users.Where(x => x.Login == user.Login && x.password == user.password).Count() != 0)
                        {
                            bw.Write(true);
                        }
                        else { bw.Write(false); }
                        break;
                    case CommandType.checkUserFull:
                        var u = bf.Deserialize(tcp.GetStream()) as User;

                        if (Users.Where(x => x.Login == u.Login || x.password == u.password || x.eMail == u.eMail || x.PhoneNum == u.PhoneNum).Count() != 0)
                        {
                            bw.Write(true);
                        }
                        else { bw.Write(false); }
                        break;
                    case CommandType.addNewUser:
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            User nUser = bf.Deserialize(tcp.GetStream()) as User;
                            Users.Add(nUser);
                            using (FileStream fs = new FileStream($"{AppDomain.CurrentDomain.BaseDirectory}users.xml", FileMode.Create))
                            {
                                xmlHandler.Serialize(fs, Users.ToList());
                            }

                            SMSSender sender = new SMSSender("vladmir.klepach@gmail.com", "1qaZ2wsX", "Noreply");
                            var result = sender.Send($"+38{nUser.PhoneNum}", "Wellcome to ViberKilla chat society, homie!");
                            sendEmail(nUser);

                        }));
                        break;
                    case CommandType.CheckFiles:
                        var clientFiles = files.Where(x => x.Key == au.Login);

                        if (clientFiles.Count() != 0)
                        {
                            bw.Write(true);
                        }
                        else bw.Write(false);
                        break;
                  
                    case CommandType.GetMessages:
                        DateTime dt = (DateTime)bf.Deserialize(tcp.GetStream());
                        var t = contract.GetMessages(dt, au.Login);
                        bf.Serialize(tcp.GetStream(), t);
                        break;
                    case CommandType.SendMessage:
                        Message data = bf.Deserialize(tcp.GetStream()) as Message; //*
                        {
                            contract.SendMessage(data);
                        }
                        foreach (var item in clientList)
                        {
                            var tt = contract.GetMessages(data.LastTime, au.Login);
                            bf.Serialize(item.Key.GetStream(), tt);
                        }
                        break;
                    case CommandType.SendFile:
                        BinaryReader netStream = new BinaryReader(tcp.GetStream());
                        byte[] bytes = new byte[ushort.MaxValue];
                        string recipient = netStream.ReadString();
                        string fName = netStream.ReadString();
                        int length = netStream.ReadInt32();
                        bytes = netStream.ReadBytes(length);
                        File.WriteAllBytes($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{fName}", bytes);
                        files.Add(recipient, fName);
                        break;
                    case CommandType.DownloadFiles:

                        clientFiles = files.Where(x => x.Key == au.Login);

                        foreach (var item in clientFiles.Select(x => x.Value).ToList())
                        {
                            string f = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{item}";
                            var bite = File.ReadAllBytes(f);
                            bw.Write(item);
                            bw.Write(bite.Length);
                            bw.Write(bite);
                        }
                        break;
                    default:
                        bye_bye = false;
                        break;
                }
            }
        }

        private void sendEmail(User newUser)
        {
            MailMessage mail = new MailMessage("kilotonns@gmail.com", newUser.eMail);
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Port = 587;
            client.Credentials = new NetworkCredential("kilotonns@gmail.com", "OD64OD233OD");
            client.EnableSsl = true;
            mail.Subject = "Registration notification";
            mail.Body = $"You was successfully registred in ViberKiller chat system!\nYour login: {newUser.Login}\nPassword: {newUser.password}";
            client.Send(mail);
        }


    }
}
