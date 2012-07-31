using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class ItemTemplateModel
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string StatsTemplate { get; set; }
        public int Pods { get; set; }
        public int ItemSetID { get; set; }
        public int Price { get; set; }
        public string Criterions { get; set; }
        public string WeaponInfos { get; set; }

        public Game.Engines.EffectEngine Engine { get; set; }
    }
}
