using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class NpcTable
    {
        public static Dictionary<int, Models.NpcModel> Cache = new Dictionary<int, Models.NpcModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @npcs@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM npc_db");
            while (reader.Read())
            {
                var npc = new Models.NpcModel()
                {
                    ID = reader.GetInt32("ID"),
                    Name = reader.GetString("Name"),
                    Skin = reader.GetInt32("Gfx"),
                    ScaleX = reader.GetInt32("ScaleX"),
                    ScaleY = reader.GetInt32("ScaleY"),
                    Sex = reader.GetInt32("Sex"),
                    Color1 = reader.GetInt32("Color1"),
                    Color2 = reader.GetInt32("Color2"),
                    Color3 = reader.GetInt32("Color3"),
                    Accessories = reader.GetString("Accessories"),
                    Clip = reader.GetInt32("Clip"),
                    Artwork = reader.GetInt32("Artwork"),
                    Bonus = reader.GetInt32("Bonus"),
                    InitQuestion = reader.GetInt32("InitQuestion"),
                    SaleItems = reader.GetString("SaleItems"),
                };
                Cache.Add(npc.ID, npc);
            }
            reader.Close();

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ npcs");
        }
    }
}
