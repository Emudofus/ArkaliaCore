using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class TriggerTable
    {
        public static List<Models.TriggerModel> Cache = new List<Models.TriggerModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @triggers@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM maps_triggers");
            while (reader.Read())
            {
                var trigger = new Models.TriggerModel()
                {
                    ID = reader.GetInt32("ID"),
                    MapID = reader.GetInt32("MapID"),
                    CellID = reader.GetInt32("CellID"),
                };
                string[] data = reader.GetString("NewMap").Split(',');
                trigger.NextMap = int.Parse(data[0]);
                trigger.NextCell = int.Parse(data[1]);
                Cache.Add(trigger);

                //Add trigger to map
                var map = Game.Controllers.MapController.GetMap(trigger.MapID);
                if (map != null)
                {
                    if (!map.Triggers.ContainsKey(trigger.CellID))
                    {
                        map.Triggers.Add(trigger.CellID, trigger);
                    }
                }
            }
            reader.Close();

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ triggers");
        }
    }
}
