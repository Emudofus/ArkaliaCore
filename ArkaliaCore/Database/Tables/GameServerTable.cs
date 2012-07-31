using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Database.Tables
{
    public static class GameServerTable
    {
        public static List<Models.GameServerModel> Cache = new List<Models.GameServerModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Load @GameServer@ ...");
            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM gameservers");
            while (reader.Read())
            {
                var gameserver = new Models.GameServerModel()
                {
                    ID = reader.GetInt32("ID"),
                    Adress = reader.GetString("ServerAdress"),
                    GamePort = reader.GetInt32("GamePort"),
                    CommunicationPort = reader.GetInt32("CommunicationPort"),
                };
                Cache.Add(gameserver);
            }
            reader.Close();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "' GameServer@ from database");
        }
    }
}
