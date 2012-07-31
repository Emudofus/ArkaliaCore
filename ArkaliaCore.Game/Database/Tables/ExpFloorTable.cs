using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public class ExpFloorTable
    {
        public static Dictionary<int, Models.ExpFloorModel> Cache = new Dictionary<int, Models.ExpFloorModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @exp floors@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM exp_floors");
            while (reader.Read())
            {
                var floor = new Models.ExpFloorModel()
                {
                    ID = reader.GetInt32("ID"),
                    Character = reader.GetInt64("Characters"),
                    Job = reader.GetInt32("Job"),
                    Mount = reader.GetInt32("Mount"),
                    PvP = reader.GetInt32("Pvp"),
                };
                Cache.Add(floor.ID, floor);
            }
            reader.Close();

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ exp floors");
        }
    }
}
