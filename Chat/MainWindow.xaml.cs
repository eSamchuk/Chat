using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        vModel vm;

        public MainWindow(string Login)
        {
            InitializeComponent();
            vm = new vModel(Login);
            this.DataContext = vm;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.OnClosing();
        }

        private void chbPrivate_Checked(object sender, RoutedEventArgs e)
        {
            if (chbPrivate.IsChecked==false)
            {
                tbRecipient.Text = String.Empty;
                btSendFile.IsEnabled = false;
            }
            else
            {
                btSendFile.IsEnabled = true;
            }
        }

        private void btSendFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                vm.fileName = new FileInfo(ofd.FileName);
            }
        }
    }
}
