using System;
using System.IO;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Contract;


namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // if (checkUser())
            {
                this.Visibility = Visibility.Hidden;
                MainWindow mWindow = new MainWindow(tbLogin.Text);
                mWindow.ShowDialog();
                this.Visibility = Visibility.Visible;
            }
            //else
            {
              //  MessageBox.Show("Authorization Error!");
            }
        }

        private bool checkUser()
        {
            var eP = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 10001);
            bool res = false;
            TcpClient tcp = new TcpClient();
            tcp.Connect(eP);
            NetworkStream nStream = tcp.GetStream();
            BinaryWriter writer = new BinaryWriter(nStream);
            BinaryReader reader = new BinaryReader(nStream);
            BinaryFormatter serializer = new BinaryFormatter();

            writer.Write((byte)CommandType.checkUser);
            serializer.Serialize(nStream, new User(tbLogin.Text, tbPass.Text));
            res = reader.ReadBoolean();
            writer.Write((byte)CommandType.Disconect);
            return res;
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Register r = new Register();
            r.ShowDialog();
            this.Visibility = Visibility.Visible;
        }
    }
}
