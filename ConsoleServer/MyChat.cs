using System;
using System.Collections.Generic;
using System.Linq;
using Contract;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer
{
    class MyChat : IContract
    {

        static List<Message> messages = new List<Message>();

        public void SendMessage(Message ob)
        {
            lock (messages)
            {
                messages.Add(ob);
            }
            Console.WriteLine(ob.ToString());
        }

        public List<Message> GetMessages(DateTime dt)
        {
            lock (messages)
            {
                return (from x in messages
                        where x.LastTime > dt
                        select x).ToList();
            }
        }

        public bool Connect(string login)
        {
            Console.WriteLine(login);
            return true;
        }

        public bool Disconect()
        {
            return true;
        }


        static MyChat()
        {
            messages.Add(new Message(new DateTime(2017, 8, 14, 15, 0, 45), "Вова", "Ложись, это гуки!"));
            messages.Add(new Message(new DateTime(2017, 8, 14, 15, 1, 5), "Андрей", "Ложись, это гуки!"));
            messages.Add(new Message(new DateTime(2017, 8, 14, 15, 2, 55), "Бальтазар", "Ложись, это гуки!"));
            messages.Add(new Message(new DateTime(2017, 8, 14, 15, 4, 12), "Гук", "Ложись, это гуки!"));
            messages.Add(new Message(new DateTime(2017, 8, 14, 15, 12, 4), "Юстас", "Ложись, это гуки!"));
        }


    }
}
