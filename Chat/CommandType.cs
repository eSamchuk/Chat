using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
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
