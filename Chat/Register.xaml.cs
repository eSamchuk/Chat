using System;
using System.Windows;
using Contract;
using System.Net;
using System.Net.Sockets;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

namespace Chat
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public User newUser { get; set; }

        public Register()
        {
            InitializeComponent();
            newUser = new User();
            this.DataContext = newUser;
            newUser.Login = "Skif";
        }

        private void btCansel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            if (registerNewUser()) this.Close();
            else resetUser();
        }

        private void resetUser()
        {
            newUser.Login = String.Empty;
            newUser.password = String.Empty;
            newUser.PhoneNum = String.Empty;
            newUser.eMail = String.Empty;
            tbPassConfirm.Text = String.Empty;
        }

        private bool registerNewUser()
        {
            bool res = false;
            var eP = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 10001);
            TcpClient tcp = new TcpClient();
            tcp.Connect(eP);
            NetworkStream nStream = tcp.GetStream();
            BinaryWriter writer = new BinaryWriter(nStream);
            BinaryReader reader = new BinaryReader(nStream);
            BinaryFormatter serializer = new BinaryFormatter();

            writer.Write((byte)CommandType.checkUserFull);
            serializer.Serialize(nStream, newUser);
            bool repeat = reader.ReadBoolean();

            if (!repeat)
            {
                writer.Write((byte)CommandType.addNewUser);
                serializer.Serialize(nStream, newUser);
                res = true;

            }
            else
            {
                MessageBox.Show("Некоректні дані!\nСпробуйте інші варіанти!");
            }

            writer.Write((byte)CommandType.Disconect);

            return res;
        }

           
    }
}
