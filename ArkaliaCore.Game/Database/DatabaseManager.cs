using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Utilities;

using Candy.Core.Database;

namespace ArkaliaCore.Game.Database
{
    public static class DatabaseManager
    {
        public static MySqlProvider Provider { get; set; }
        public static object Locker = new object();

        public static void InitConnection()
        {
            try
            {
                Logger.Infos("Connecting to @database@ ...");
                Utilities.ConsoleStyle.EnableLoadingSymbol();
                Provider = new MySqlProvider(
                            Settings.GetSettings.GetStringElement("Database", "Host"),
                            Settings.GetSettings.GetStringElement("Database", "Name"),
                            Settings.GetSettings.GetStringElement("Database", "Username"),
                            Settings.GetSettings.GetStringElement("Database", "Password"));
                Provider.Connect();
                Utilities.ConsoleStyle.DisabledLoadingSymbol();
                Logger.Infos("Connected to the @database@ !");
            }
            catch (Exception e)
            {
                Logger.Error("Can't @connect to database@ : " + e.ToString());
            }
        }
    }
}
