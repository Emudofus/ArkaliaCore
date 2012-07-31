using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SilverSock;

namespace ArkaliaCore.Game.Network.Realm
{
    public class SyncServer
    {
        public static SilverServer Server { get; set; }
        public static List<SyncClient> SecuredRealm = new List<SyncClient>();

        public static void Start()
        {
            Server = new SilverServer(Utilities.Settings.GetSettings.GetStringElement("Sync", "Host"),
                                                Utilities.Settings.GetSettings.GetIntElement("Sync", "Port"));
            Server.OnListeningFailedEvent += new SilverEvents.ListeningFailed(Server_OnListeningFailedEvent);
            Server.OnListeningEvent += new SilverEvents.Listening(Server_OnListeningEvent);
            Server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(Server_OnAcceptSocketEvent);
            Server.WaitConnection();
        }

        private static void Server_OnAcceptSocketEvent(SilverSocket socket)
        {
            try
            {
                var server = new SyncClient(socket);
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't @synchronize@ server : " + e.ToString());
            }
        }

        private static void Server_OnListeningEvent()
        {
            Utilities.Logger.Infos("SyncServer listen on @" + Utilities.Settings.GetSettings.GetStringElement("Sync", "Host") + ":" +
                                                                               Utilities.Settings.GetSettings.GetIntElement("Sync", "Port") + "@");
        }

        private static void Server_OnListeningFailedEvent(Exception ex)
        {
            Utilities.Logger.Error("Can't listen for the @sync server@");
        }

        public static void SendToSecuredRealm(Interop.Crystal.CrystalPacket packet)
        {
            lock (SecuredRealm)
            {
                SecuredRealm.ForEach(x => x.Send(packet));
            }
        }
    }
}
