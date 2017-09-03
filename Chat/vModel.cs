using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Contract;

namespace Chat
{
    class vModel : INotifyPropertyChanged
    {
        public Client client { get; set; }

        private ObservableCollection<Message> _messagesCol;
        public ObservableCollection<Message> MessageCol
        {
            get { return _messagesCol; }
            set
            {
                if (_messagesCol == value) return;
                _messagesCol = value;
                OnPropertyChanged();
            }
        }

        private relayCommand _comm;
        public relayCommand sendComm
        {
            get { return _comm; }
            set { _comm = value; }
        }

        private relayCommand _closeComm;
        public relayCommand closeComm
        {
            get { return _closeComm; }
            set { _closeComm = value; }
        }

        private relayCommand _sendFileComm;
        public relayCommand sendFileComm
        {
            get { return _sendFileComm; }
            set { _sendFileComm = value; }
        }

        private relayCommand _getFileComm;
        public relayCommand getFileComm
        {
            get { return _getFileComm; }
            set { _getFileComm = value; }
        }

        private relayCommand _disconnectComm;
        public relayCommand disconnectComm
        {
            get { return _disconnectComm; }
            set { _disconnectComm = value; }
        }


        private relayCommand _logComm;
        public relayCommand logComm
        {
            get { return _logComm; }
            set { _logComm = value; }
        }

        private string _recipient;
        public string  Recipient
        {
            get { return _recipient; }
            set { _recipient = value; }
        }

        private FileInfo _fileName;
        public FileInfo fileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private String _login;
        public String Login
        {
            get { return _login; }
            set
            {
                if (_login == value) return;
                _login = value;
                OnPropertyChanged();
            }
        }

        private String _messageText;
        public String MessageText
        {
            get { return _messageText; }
            set
            {
                if (_messageText == value) return;
                _messageText = value;
                OnPropertyChanged();
            }
        }

        private bool bye_bye = true;

        public DispatcherTimer Timer { get; set; }

        public vModel(string au)
        {
            client = new Client();
            MessageCol = new ObservableCollection<Message>();
            Login = au;
            client.Connect(Login);
            sendComm = new relayCommand(param => client.SendMessage(new Message(DateTime.Now, Login, MessageText, Recipient)));
            sendFileComm = new relayCommand(param => client.SendFile(Recipient, fileName));
            closeComm = new relayCommand(param => this.OnClosing());
            disconnectComm = new relayCommand(param => client.Disconect());
            getFileComm = new relayCommand(param => client.DownloadFiles());
            Thread th = new Thread(() => listenToServer());
            th.Start();
        }

        private void listenToServer()
        {
            while (bye_bye)
            {
                if (client.IsDataAvailable)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (var item in client.listen())
                        {
                            MessageCol.Add(item);
                        }
                    }));
                }
            }
        }

    
        public void OnClosing()
        {
            bye_bye = false;
            //Timer.Stop();
            client.Disconect();
        }


        #region PropertyChangedMembers
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
