using System;
using System.Collections.Generic;
using System.Linq;
using Contract;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Servak
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
        }

        public List<Message> GetMessages(DateTime dt, string privateLog)
        {
            lock (messages)
            {
                var t = messages.Where(x => x.LastTime >= dt && x.SentTo == null);
                var t2 = messages.Where(x => x.LastTime >= dt && x.SentTo == privateLog);
                var t3 = messages.Where(x => x.LastTime >= dt && x.Login == privateLog);
                t = t.Concat(t2);
                t = t.Concat(t3);
                return t.OrderBy(x => x.LastTime).Distinct().ToList();
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

        public void SendFile(string au, FileInfo file)
        {
            throw new NotImplementedException();
        }

        static MyChat()
        {
            //messages.Add(new Message(DateTime.Now - new TimeSpan(1, 0, 15), "1", "Ложись, это гуки!", "2"));
            //messages.Add(new Message(DateTime.Now - new TimeSpan(1, 0, 12), "2", "Где?", "1"));
            //messages.Add(new Message(DateTime.Now - new TimeSpan(1, 0, 8), "1", "На 150!", "2"));
            //messages.Add(new Message(DateTime.Now - new TimeSpan(1, 0, 5), "2", "Вижу!", "1"));
        }


    }
}
