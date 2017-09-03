using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IContract
    {
        void SendMessage(Message ob);
        List<Message> GetMessages(DateTime dt, string login);

        void SendFile(String recipient, FileInfo file);

        bool Connect(String login);

        bool Disconect();
    }

   
}
