using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = ("Arkalia Core v" + Definitions.CoreVersion + " (Game)");
            Definitions.StartTime = Environment.TickCount;
            Utilities.Logger.DrawAscii();
            Utilities.Logger.Initialize();
            Utilities.ConsoleStyle.InitConsole();
            Utilities.Logger.Infos("Start @Arkalia Core@ ...");

            Definitions.StartTime = Environment.TickCount;

            Utilities.Settings.Load();

            Utilities.Logger.Stage("Static");
            Game.Chat.ChannelManager.Initialize();
            Game.Jobs.JobsManager.Initialize();
            Game.Commands.CommandManager.Initialize();

            Utilities.Logger.Stage("Database");
            Database.DatabaseManager.InitConnection();
            Database.Tables.AccountInformationsTable.Load();
            Database.Tables.ItemTemplateTable.Load();
            Database.Tables.MapTable.Load();
            Database.Tables.TriggerTable.Load();
            Database.Tables.WaypointTable.Load();
            Database.Tables.NpcTable.Load();
            Database.Tables.NpcPosTable.Load();
            Database.Tables.NpcDialogTable.Load();
            Database.Tables.BreedTable.Load();
            Database.Tables.ExpFloorTable.Load();
            Database.Tables.CharacterTable.Load();
            Database.Tables.WorldItemTable.Load();

            Utilities.Logger.Stage("Plugins");
            Modules.Scripting.ScriptKernel.Load();

            Utilities.Logger.Stage("Network");
            Network.Game.GameServer.Start();
            Network.Realm.SyncServer.Start();
            Network.Web.WebServer.Start();

            Utilities.Logger.Infos("Arkalia started in @" + (Environment.TickCount - Definitions.StartTime) + "ms@");
            Definitions.StartTime = Environment.TickCount;//Reset for uptime
            Definitions.IsLoaded = true;

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
