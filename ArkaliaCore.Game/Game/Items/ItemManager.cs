using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Items
{
    public class ItemManager
    {
        public static List<int> WeaponsEffectsID = new List<int>() { 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101 };

        public static bool IsWeaponEffect(int effect)
        {
            lock (WeaponsEffectsID)
            {
                return WeaponsEffectsID.Contains(effect);
            }
        }

        public static int GetRandomJet(string jet)
        {
            try
            {
                var num = 0;
                var des = int.Parse(jet.Split('d')[0]);
                var faces = int.Parse(jet.Split('d')[1].Split('+')[0]);
                var add = int.Parse(jet.Split('d')[1].Split('+')[1]);
                for (int a = 0; a < des; a++)
                {
                    num += Utilities.Basic.RandomNumber(1, faces);
                }
                num += add;
                return num;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public static Database.Models.WorldItemModel GenerateItem(int owner, Database.Models.ItemTemplateModel template, int quantity = 1, bool max = false)
        {
            var item = new Database.Models.WorldItemModel();
            item.ID = Database.Tables.WorldItemTable.TempID;
            item.Owner = owner;
            item.Position = -1;
            item.Quantity = quantity;
            item.TemplateID = template.ID;
            item.Engine = new Engines.EffectEngine(template.Engine.GenerateEffects(max));
            item.Effects = item.Engine.ToString();
            return item;
        }

        public static Database.Models.ItemTemplateModel GetTemplate(int id)
        {
            lock (Database.Tables.ItemTemplateTable.Cache)
            {
                if (Database.Tables.ItemTemplateTable.Cache.ContainsKey(id))
                {
                    return Database.Tables.ItemTemplateTable.Cache[id];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
