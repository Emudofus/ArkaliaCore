using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Alchemy;
using Alchemy.Classes;

namespace ArkaliaCore.Game.Network.Web
{
    public class WebServer
    {
        public static WebSocketServer Server { get; set; }
        public static List<WebClientConnection> Clients = new List<WebClientConnection>();

        public static void Start()
        {
            Server = new WebSocketServer(81, IPAddress.Any);
            Server.OnConnect = OnConnected;
            Server.Start();
            Utilities.Logger.Infos("Websocket server started !");
        }

        public static void OnConnected(UserContext context)
        {
            Utilities.Logger.Debug("New websocket connection : " + context.ClientAddress.ToString());
            var client = new WebClientConnection(context);
            lock (Clients)
            {
                Clients.Add(new WebClientConnection(context));
            }
        }
    }
}
