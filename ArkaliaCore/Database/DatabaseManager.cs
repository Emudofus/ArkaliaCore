using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Realm.Utilities;

using Candy.Core.Database;

namespace ArkaliaCore.Realm.Database
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
                Provider = new MySqlProvider(
                            Settings.GetSettings.GetStringElement("Database", "Host"),
                            Settings.GetSettings.GetStringElement("Database", "Name"),
                            Settings.GetSettings.GetStringElement("Database", "Username"),
                            Settings.GetSettings.GetStringElement("Database", "Password"));
                Provider.Connect();
                Logger.Infos("Connected to the @database@ !");
            }
            catch (Exception e)
            {
                Logger.Error("Can't @connect to database@ : " + e.ToString());
            }
        }
    }
}
