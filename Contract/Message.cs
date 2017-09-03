using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Contract
{
    [Serializable]
    public class Message
    {
        public DateTime LastTime { get; set; }
        public String Text { get; set; }
        public String Login { get; set; }
        public String SentTo { get; set; }

        public override string ToString()
        {
            return String.Format("LastTime:{0} Login{1} Text:{2} SendTo{3}{4}", LastTime, Login, Text, SentTo, Environment.NewLine);
        }

        public Message() { }

        public Message(DateTime dt, string Login, string Text, string sentTo = "")
        {
            this.LastTime = dt;
            this.Login = Login;
            this.Text = Text;
            this.SentTo = sentTo;
        }
    }

    public enum CommandType : byte
    {
        SendMessage,
        GetMessages,
        SendFile,
        Connect,
        Disconect,
        CheckFiles,
        DownloadFiles,
        addNewUser,
        checkUser,
        checkUserFull
    }
}
