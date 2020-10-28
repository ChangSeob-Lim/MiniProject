using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebRequestJsonTest
{
    public class WebSockBasicTest
    {
        ClientWebSocket sock = null;
        

        public WebSockBasicTest()
        {
            StartConnect();
        }

        public void StartConnect()
        {
            sock = new ClientWebSocket();
            sock.ConnectAsync(new Uri("ws://localhost:8080/websocket"), CancellationToken.None);
        }
    }
}
