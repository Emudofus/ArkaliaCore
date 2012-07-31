using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class ItemTemplateTable
    {
        public static Dictionary<int, Models.ItemTemplateModel> Cache = new Dictionary<int, Models.ItemTemplateModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @item templates@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM item_template");
            while (reader.Read())
            {
                var template = new Models.ItemTemplateModel()
                {
                    ID = reader.GetInt32("id"),
                    Type = reader.GetInt32("type"),
                    Name = reader.GetString("name"),
                    Level = reader.GetInt32("level"),
                    StatsTemplate = reader.GetString("statsTemplate"),
                    Pods = reader.GetInt32("pod"),
                    ItemSetID = reader.GetInt32("panoplie"),
                    Price = reader.GetInt32("prix"),
                    Criterions = reader.GetString("condition"),
                    WeaponInfos = reader.GetString("armesInfos"),
                };
                template.Engine = new Game.Engines.EffectEngine(template.StatsTemplate);
                template.Engine.Load();
                Cache.Add(template.ID, template);
            }
            reader.Close();

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ item templates with @'" + Statistics.EffectLoadedCount + "'@ effects");
        }
    }
}
