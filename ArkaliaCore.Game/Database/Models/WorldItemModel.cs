using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class WorldItemModel
    {
        public int ID { get; set; }
        public int Owner { get; set; }
        public int TemplateID { get; set; }
        public int Position { get; set; }
        public int Quantity { get; set; }
        public string Effects { get; set; }

        public Game.Engines.EffectEngine Engine { get; set; }

        public ItemTemplateModel Template
        {
            get
            {
                return Game.Items.ItemManager.GetTemplate(this.TemplateID);
            }
        }

        /// <summary>
        /// Check if the stuff is on the character
        /// </summary>
        public bool IsStuffed { get { return this.Position != -1; } }
    }
}
