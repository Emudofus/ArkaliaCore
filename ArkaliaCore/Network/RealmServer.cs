using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SilverSock;

namespace ArkaliaCore.Realm.Network
{
    public class RealmServer
    {
        public static SilverServer Server { get; set; }
        public static List<RealmClient> Clients { get; set; }

        public static void Start()
        {
            Clients = new List<RealmClient>();
            Server = new SilverServer(Utilities.Settings.GetSettings.GetStringElement("Realm", "Host"),
                                                Utilities.Settings.GetSettings.GetIntElement("Realm", "Port"));
            Server.OnListeningEvent += new SilverEvents.Listening(Server_ServerStarted);
            Server.OnListeningFailedEvent += new SilverEvents.ListeningFailed(Server_ServerStopped);
            Server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(Server_OnAcceptSocketEvent);
            Server.WaitConnection();
        }

        private static void Server_OnAcceptSocketEvent(SilverSocket socket)
        {
            lock (Clients)
            {
                try
                {
                    Utilities.Logger.Infos("Client @connected@ on realm server @<" + socket.IP + ">@");
                    var client = new RealmClient(socket);
                    Clients.Add(client);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("@Can't accept@ client : " + e.ToString());
                }
            }
        }

        private static void Server_ServerStopped(Exception e)
        {
            Utilities.Logger.Error("Can't listen for the @realm server@");
        }

        private static void Server_ServerStarted()
        {
            Utilities.Logger.Infos("RealmServer listen on @" + Utilities.Settings.GetSettings.GetStringElement("Realm", "Host") + ":" +
                                                                                Utilities.Settings.GetSettings.GetIntElement("Realm", "Port") + "@");
        }
    }
}
