using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = ("Arkalia Core v" + Definitions.CoreVersion + " (Realm)");
            Utilities.Logger.DrawAscii();
            Utilities.Logger.Infos("Start @Arkalia Core@ ...");

            Definitions.StartTime = Environment.TickCount;

            Utilities.Settings.Load();
            Database.DatabaseManager.InitConnection();
            Database.Tables.GameServerTable.Load();
            Database.Tables.AccountCharacterTable.Load();
            Managers.MultiServerManager.InitSynchronizer();
            Network.RealmServer.Start();

            Utilities.Logger.Infos("Arkalia started in @" + (Environment.TickCount - Definitions.StartTime) + "ms@");

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
