using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class WaypointTable
    {
        public static List<Models.WaypointModel> Cache = new List<Models.WaypointModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @waypoints@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM zaaps_data");
            while (reader.Read())
            {
                var waypoint = new Models.WaypointModel()
                {
                    ID = reader.GetInt32("ID"),
                    MapID = reader.GetInt32("MapID"),
                    CellID = reader.GetInt32("CellID"),
                };
                Cache.Add(waypoint);
            }
            reader.Close();

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ waypoints");
        }
    }
}
