using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Realm.Interop.Crystal;

namespace ArkaliaCore.Realm.Sync
{
    public static class SynchronizerHandler
    {
        public static void HandleHelloKey(WorldSynchronizer sync)
        {
            sync.Send(new Interop.Crystal.Packets.SecureKeyMessage(
                Utilities.Settings.GetSettings.GetStringElement("Realm", "SecureKey"),
                Utilities.Settings.GetSettings.GetStringElement("Realm", "Name")));
        }

        public static void HandleConnectionPlayer(WorldSynchronizer sync, CrystalPacket packet)
        {
            string username = packet.Reader.ReadString();
            lock (Managers.MultiServerManager.ConnectedAccounts)
            {
                if (!Managers.MultiServerManager.ConnectedAccounts.Contains(username))
                {
                    Managers.MultiServerManager.ConnectedAccounts.Add(username);
                }
            }
            Database.Tables.AccountTable.UpdateLogged(username, 1);
            Utilities.Logger.Infos("Player @'" + username + "'@ connected on server @'" + sync.Server.ID + "'@");
        }

        public static void HandleDisconnectionPlayer(WorldSynchronizer sync, CrystalPacket packet)
        {
            string username = packet.Reader.ReadString();
            lock (Managers.MultiServerManager.ConnectedAccounts)
            {
                if (Managers.MultiServerManager.ConnectedAccounts.Contains(username))
                {
                    Managers.MultiServerManager.ConnectedAccounts.Remove(username);
                }
            }
            Database.Tables.AccountTable.UpdateLogged(username, 0);
            Utilities.Logger.Infos("Player @'" + username + "'@ disconnected on server @'" + sync.Server.ID + "'@");
        }

        public static void HandleShopPointsUpdate(WorldSynchronizer sync, CrystalPacket packet)
        {
            string username = packet.Reader.ReadString();
            int points = packet.Reader.ReadInt32();
            Database.Tables.AccountTable.UpdatePoints(username, points);
            Utilities.Logger.Infos("Shop points updated for player @'" + username + "'@");
        }

        public static void HandleCharacterCreation(WorldSynchronizer sync, CrystalPacket packet)
        {
            int owner = packet.Reader.ReadInt32();
            string name = packet.Reader.ReadString();
            int server = sync.Server.ID;
            var informations = new Database.Models.AccountCharacterModel()
            {
                ID = Helpers.GuidHelper.GetAvailableCharacterID(),
                Name = name,
                Owner = owner,
                Server = server
            };
            lock (Database.Tables.AccountCharacterTable.Cache)
                Database.Tables.AccountCharacterTable.Cache.Add(informations);
            Database.Tables.AccountCharacterTable.Insert(informations);
            Utilities.Logger.Infos("Player @'" + name + "'@ was created on server @'" + server + "'@");
        }

        public static void HandleCharacterDeletion(WorldSynchronizer sync, CrystalPacket packet)
        {
            string name = packet.Reader.ReadString();
            lock (Database.Tables.AccountCharacterTable.Cache)
            {
                var character = Database.Tables.AccountCharacterTable.Cache.FirstOrDefault(x => x.Name == name);
                Database.Tables.AccountCharacterTable.Cache.Remove(character);
                Database.Tables.AccountCharacterTable.Delete(character);
            }
            Utilities.Logger.Infos("Player @'" + name + "'@ was deleted on server @'" + sync.Server.ID + "'@");
        }
    }
}
