using System;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using Contract;


namespace Servak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Server sr { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            sr = new Server();
            this.DataContext = sr;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
           // sr.disconnectAll();
        }
    }
}
