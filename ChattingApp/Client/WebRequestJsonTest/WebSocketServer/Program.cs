using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSockServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebSocketServer webSocketServer = null; 
            Int32 port = 4649;
            webSocketServer = new WebSocketServer(port);
            webSocketServer.AddWebSocketService<Chat>("/Chat");
            webSocketServer.Start();
            System.Console.WriteLine("서버시작\n");
        }

        public void addText(string text)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                string myText = Console.Text;
                myText += text;
                Console.Text = myText;
            }));
        }
    }
}
