using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class MapTable
    {
        public static Dictionary<int, Models.MapModel> Cache = new Dictionary<int, Models.MapModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @maps@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM maps_data");
            while (reader.Read())
            {
                var map = new Models.MapModel()
                {
                    ID = reader.GetInt32("id"),
                    Date = reader.GetString("date"),
                    Width = reader.GetInt32("width"),
                    Height = reader.GetInt32("heigth"),
                    Places = reader.GetString("places"),
                    DecryptKey = reader.GetString("decryptkey"),
                    MapData = reader.GetString("mapData"),
                    Cells = reader.GetString("cells"),
                    Monsters = reader.GetString("monsters"),
                    Capabilities = reader.GetInt32("capabilities"),
                    Mappos = reader.GetString("mappos"),
                    NumGroup = reader.GetInt32("numgroup"),
                    GroupMaxSize = reader.GetInt32("groupmaxsize"),
                    SpawnEmitters = reader.GetString("spawnemitters")
                };
                Cache.Add(map.ID, map);
            }
            reader.Close();
            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ maps");
        }
    }
}
