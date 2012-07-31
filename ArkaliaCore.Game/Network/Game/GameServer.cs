using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SilverSock;

namespace ArkaliaCore.Game.Network.Game
{
    public static class GameServer
    {
        public static SilverServer Server { get; set; }
        public static List<GameClient> Clients = new List<GameClient>();

        public static void Start()
        {
            Server = new SilverServer(Utilities.Settings.GetSettings.GetStringElement("Game", "Host"),
                                                Utilities.Settings.GetSettings.GetIntElement("Game", "Port"));
            Server.OnListeningFailedEvent += new SilverEvents.ListeningFailed(Server_OnListeningFailedEvent);
            Server.OnListeningEvent += new SilverEvents.Listening(Server_OnListeningEvent);
            Server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(Server_OnAcceptSocketEvent);
            Server.WaitConnection();
        }

        private static void Server_OnAcceptSocketEvent(SilverSocket socket)
        {
            try
            {
                lock (Clients)
                {
                    Utilities.Logger.Infos("Client @connected@ on game server @<" + socket.IP + ">@");
                    var client = new GameClient(socket);
                    Clients.Add(client);
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't @accept connection@ on game server : " + e.ToString());
            }
        }

        private static void Server_OnListeningEvent()
        {
            Utilities.Logger.Infos("GameServer listen on @" + Utilities.Settings.GetSettings.GetStringElement("Game", "Host") + ":" +
                                                                               Utilities.Settings.GetSettings.GetIntElement("Game", "Port") + "@");
        }

        private static void Server_OnListeningFailedEvent(Exception ex)
        {
            Utilities.Logger.Error("Can't listen for the @game server@");
        }
    }
}
