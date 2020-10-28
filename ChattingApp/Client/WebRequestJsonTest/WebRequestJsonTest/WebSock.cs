using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace WebRequestJsonTest
{
    public class WebSock
    {
        public static WebSocket WebConnect(WebSocket client)
        {
            string msg = string.Empty;

            /////////////// 1차 ///////////////////
            //WebSocket client = new WebSocket(url: "ws://localhost:8080/websocket");
            //if(client == null)
            //    client = new WebSocket("ws://localhost:8080/broadsocket");
            
            //client.Connect();

            //client.Send("Hello");

            //client.OnMessage += (sender, e) =>
            //{
            //    msg = e.Data;
            //};
            /////////////// 1차 ///////////////////

            if (client == null)
            {
                client = new WebSocket("ws://localhost:8080/websocket");
                
            }

            client.Connect();

            //client.OnMessage += (sender, e) =>
            //{
            //    msg = e.Data;
            //};

            client.Send("Test Message");

            return client; 
        }
    }
}
