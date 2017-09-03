using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace ConsoleServer
{
    class Server
    {
        MyChat contract = new MyChat();

        TcpListener serv;
        TcpClient client;
        public Server()
        {
            serv = new TcpListener(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 12345));
            serv.Start();
            client = serv.AcceptTcpClient();
            new Thread(ListenerClient).Start(client);
        }

        public void ListenerClient(Object ob)
        {
            TcpClient tcp = ob as TcpClient;
            CommandType comand = new CommandType();
            BinaryFormatter bf = new BinaryFormatter();

            //отримання з клієнта
            Authentification au = bf.Deserialize(tcp.GetStream()) as Authentification;

            bool result = contract.Connect(au.Login);
            BinaryWriter bw = new BinaryWriter(tcp.GetStream());
            //відсилається назад
            bw.Write(result);
            BinaryReader br = new BinaryReader(tcp.GetStream());

            if (!result)
            {
                return;
            }
            bool bye_bye = true;
            while (bye_bye)
            {
                try
                {
                    comand = (CommandType)br.ReadByte();
                }
                catch(Exception e)
                {
                    string s = e.Message;
                }

                switch (comand)
                {
                    case CommandType.Connect:
                        break;
                    case CommandType.Disconect:
                        tcp.Close();
                        break;
                    case CommandType.GetMessages:
                        DateTime dt = (DateTime)bf.Deserialize(tcp.GetStream());
                        var t = contract.GetMessages(dt);
                        bf.Serialize(tcp.GetStream(), t);
                        break;
                    case CommandType.SendMessage:
                        Message data = bf.Deserialize(tcp.GetStream()) as Message; //*
                        contract.SendMessage(data);
                        break;
                    default:
                        bye_bye = false;
                        break;
                }
            }
        }
    }
}
