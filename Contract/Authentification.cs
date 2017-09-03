using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Contract
{
    [Serializable]
    public class User : INotifyPropertyChanged
    {
        private String _Login;
        public String Login
        {
            get { return _Login; }
            set
            {
                if (_Login == value) return;
                _Login = value;
                OnPropertyChanged();
            }
        }

        private String _eMail;
        public String eMail
        {
            get { return _eMail; }
            set
            {
                if (_eMail == value) return;
                _eMail = value;
                OnPropertyChanged();
            }
        }

        private String _pass;
        public String password
        {
            get { return _pass; }
            set
            {
                if (_pass == value) return;
                _pass = value;
                OnPropertyChanged();
            }
        }

        private String _PhoneNum;

        public String PhoneNum
        {
            get { return _PhoneNum; }
            set
            {
                if (_PhoneNum == value) return;
                _PhoneNum = value;
                OnPropertyChanged();
            }
        }

        private bool _isConnected;

        public Boolean isConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected == value) return;
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public User() { }
        public User(string logIn)
        {
            this.Login = logIn;
        }
        public User(string logIn, string pass)
        {
            this.Login = logIn;
            this.password = pass;
        }
        public User(string logIn, string pass, string eMail, string Phone)
        {
            this.Login = logIn;
            this.password = pass;
            this.eMail = eMail;
            this.PhoneNum = Phone;
            this.isConnected = false;
        }

        public override string ToString()
        {
            return $"{Login} {isConnected}";
        }

        #region INotifyPropertyChanged members
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
