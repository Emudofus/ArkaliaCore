using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alchemy;
using Alchemy.Classes;

namespace ArkaliaCore.Game.Network.Web
{
    public class WebClientConnection
    {
        public UserContext Socket { get; set; }

        public WebClientConnection(UserContext context)
        {
            this.Socket = context;
            this.Socket.SetOnDisconnect(OnDisconnect);
            this.Socket.SetOnReceive(OnReceived);
        }

        public void OnDisconnect(UserContext context)
        {
            lock (WebServer.Clients)
            {
                WebServer.Clients.Remove(this);

                Utilities.Logger.Debug("Webclient disconnected !");
            }
        }

        public void OnReceived(UserContext context)
        {
            try
            {
                var packet = context.DataFrame.ToString();
                Utilities.Logger.Debug("Received from websocket : " + packet);
                this.handlePacket(packet);
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't parse packet : " + e.ToString());
            }
        }

        public void Send(string packet)
        {
            try
            {
                this.Socket.Send(packet);
                Utilities.Logger.Debug("Send websocket packet : " + packet);
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't send packet : " + e.ToString());
            }
        }

        private void handlePacket(string packet)
        {
            switch (packet[0])
            {
                case 'U':
                    switch (packet[1])
                    {
                        case 'p':
                            this.Send("Up" + Utilities.Basic.GetUptime());
                            break;

                        case 'c':
                            this.Send("Uc" + Network.Game.GameServer.Clients.Count);
                            break;
                    }
                    break;
            }
        }

        #region API Methods



        #endregion
    }
}
