using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Managers
{
    public static class MultiServerManager
    {
        public static Dictionary<int, Sync.WorldSynchronizer> Synchronizers = new Dictionary<int, Sync.WorldSynchronizer>();
        public static List<string> ConnectedAccounts = new List<string>();

        public static void InitSynchronizer()
        {
            Utilities.Logger.Infos("Start @Synchronizer@ ...");
            foreach (var server in Database.Tables.GameServerTable.Cache)
            {
                var sync = new Sync.WorldSynchronizer(server);
                Synchronizers.Add(server.ID, sync);
                sync.StartSync();
            }
            Utilities.Logger.Infos("@Synchronizer@ started !");
        }

        public static void SendServerState(Network.RealmClient client)
        {
            StringBuilder packet = new StringBuilder("AH");
            lock (Database.Tables.GameServerTable.Cache)
            {
                foreach (var server in Database.Tables.GameServerTable.Cache)
                {
                    if (packet.ToString() == "")
                    {
                        packet.Append(server.ID).Append(";").Append((int)server.State).Append(";").Append((server.ID *75).ToString()).Append(";1");
                    }
                    else
                    {
                        packet.Append("|").Append(server.ID).Append(";").Append((int)server.State).Append(";").Append((server.ID * 75).ToString()).Append(";1");
                    }
                }
            }
            client.Send(packet.ToString());
        }

        public static List<Database.Models.AccountCharacterModel> GetCharactersInformations(Database.Models.AccountModel account)
        {
            lock (Database.Tables.AccountCharacterTable.Cache)
            {
                return Database.Tables.AccountCharacterTable.Cache.FindAll(x => x.Owner == account.ID);
            }
        }

        public static void SendCharacterCount(Network.RealmClient client)
        {
            StringBuilder packet = new StringBuilder();
            lock (Database.Tables.GameServerTable.Cache)
            {
                foreach (var server in Database.Tables.GameServerTable.Cache)
                {
                    if (packet.ToString() == "")
                    {
                        packet.Append(server.ID + "," + client.Characters.FindAll(x => x.Server == server.ID).Count);
                    }
                    else
                    {
                        packet.Append("|" + server.ID + "," + client.Characters.FindAll(x => x.Server == server.ID).Count);
                    }
               }
            }
            client.Send("AxK31600000000000|" + packet.ToString());
        }

        public static void RefreshState()
        {
            lock (Network.RealmServer.Clients)
            {
                Network.RealmServer.Clients.ForEach(x => SendServerState(x));
                Utilities.Logger.Infos("Servers state refreshed !");
            }
        }

        public static Sync.WorldSynchronizer GetSync(int id)
        {
            lock(Synchronizers)
                return Synchronizers[id];
        }

        public static void Send(Interop.Crystal.CrystalPacket packet)
        {
            lock (Synchronizers)
            {
                try
                {
                    Synchronizers.ToList().ForEach(x => x.Value.Send(packet));
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't send packet to all server : " + e.ToString());
                }
            }
        }
    }
}
