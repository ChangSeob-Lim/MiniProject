using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WebSocketSharp.NetCore;

namespace ChattingApp.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        string msg = null;
        WebSocket client = null;

        public MainView()
        {
            InitializeComponent();

            TestFunction();

            InitTimer();
        }

        private void InitTimer()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += SetMsg;
        }

        private void SetMsg(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                CheckText.Text += msg;
                msg = null;
            }
        }

        private void TestFunction()
        {
            Send.Click += SendMsg;
            Close.Click += SocketClose;
        }

        private void SendMsg(object sender, RoutedEventArgs e)
        {
            if(client == null)
            {
                client = new WebSocket("ws://localhost:8080/chatting/server");
                timer.Start();
            }

            //client.OnOpen += (sender, e) => {
            //    client.Send("Hello");
            //};

            client.Connect();

            client.Send("Hello");

            client.OnMessage += GetMsg;
        }

        private void GetMsg(object sender, MessageEventArgs e)
        {
            msg = e.Data + "\n";
        }

        private void SocketClose(object sender, RoutedEventArgs e)
        {
            if (client != null)
                client.Close();
            client = null;
            timer.Stop();
        }
    }
}
