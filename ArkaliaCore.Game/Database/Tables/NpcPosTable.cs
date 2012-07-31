using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class NpcPosTable
    {
        public static List<Models.NpcPosModel> Cache = new List<Models.NpcPosModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @npcs pos@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM npc_pos");
            while (reader.Read())
            {
                var pos = new Models.NpcPosModel()
                {
                    ID = reader.GetInt32("ID"),
                    NpcID = reader.GetInt32("NpcID"),
                    MapID = reader.GetInt32("MapId"),
                    CellID = reader.GetInt32("CaseId"),
                    Dir = reader.GetInt32("Orientation"),
                };
                Cache.Add(pos);
            }
            reader.Close();

            foreach (var pos in Cache)
            {
                var npcInstance = new Game.Npcs.NpcInstance(pos, pos.Map);
                npcInstance.RegisterToMap();
            }

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ npcs pos");
        }
    }
}
