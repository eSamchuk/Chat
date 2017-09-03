using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Contract;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chat
{
    class Client : IContract, INotifyPropertyChanged
    {
        TcpClient tcp;
        NetworkStream nStream;
        BinaryFormatter serializer;

        private bool _isConnected;
        public bool Connected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected == value) return;
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        private bool _isFiles;

        public bool isFileWaiting
        {
            get { return _isFiles; }
            set
            {
                if (_isFiles == value) return;
                _isFiles = value;
                OnPropertyChanged();
            }
        }
        public User Au { get; set; }
        public BinaryWriter writer;
        public BinaryReader reader;

        public Client()
        {
            //tcp = new TcpClient();
            serializer = new BinaryFormatter();
        }

        public bool Connect(string login)
        {
            tcp = new TcpClient();
            IPEndPoint eP = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 10001);
            tcp.Connect(eP);
            nStream = tcp.GetStream();
            writer = new BinaryWriter(nStream);
            reader = new BinaryReader(nStream);
            writer.Write((byte)CommandType.Connect);
            serializer.Serialize(nStream, new User(login));
            Connected = reader.ReadBoolean();
            if (Connected)
            {
                writer.Write((byte)CommandType.CheckFiles); //відправка даних
                isFileWaiting = reader.ReadBoolean();
            }
            return true;
        }

        public List<Message> listen()
        {
            var t = serializer.Deserialize(nStream) as List<Message>;
            return t;
        }


        public void SendMessage(Message ob)
        {
            writer.Write((byte)CommandType.SendMessage); //відправка даних
            serializer.Serialize(nStream, ob); //*
        }

        public void SendFile(String recipient, FileInfo file)
        {
            writer.Write((byte)CommandType.SendFile); //відправка даних
            byte[] buff = File.ReadAllBytes(file.FullName);
            writer.Write(recipient);
            writer.Write(file.Name);
            writer.Write(buff.Length);
            writer.Write(buff);
        }

        public List<Message> GetMessages(DateTime dt, string Recipient)
        {
            writer.Write((byte)CommandType.GetMessages);
            serializer.Serialize(nStream, dt); //*
            List<Message> t = serializer.Deserialize(nStream) as List<Message>;
            return t;
        }

        public void DownloadFiles()
        {
            writer.Write((byte)CommandType.DownloadFiles);
            string name = reader.ReadString();
            byte[] buff = reader.ReadBytes(reader.ReadInt32());
            File.WriteAllBytes($"{ AppDomain.CurrentDomain.BaseDirectory}\\Files\\{name}", buff);
            isFileWaiting = false;
        }

        public bool Disconect()
        {
            if (tcp.Connected)
            {
                var writer = new BinaryWriter(nStream);
                writer.Write((byte)CommandType.Disconect); 
                nStream.Close();
                tcp.Close();
                Connected = false;
            }
            return true;
        }

        public bool IsDataAvailable { get {return nStream.DataAvailable; } }

        #region PropertyChangedMembers
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

      
        #endregion


    }
}
