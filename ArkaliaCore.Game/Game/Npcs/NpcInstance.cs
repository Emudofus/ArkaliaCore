using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Npcs
{
    public class NpcInstance
    {
        public int ID { get; set; }
        public Database.Models.NpcPosModel Positions { get; set; }
        public Database.Models.MapModel Map { get; set; }

        public NpcInstance(Database.Models.NpcPosModel pos, Database.Models.MapModel map)
        {
            this.Positions = pos;
            this.Map = map;
        }

        public void RegisterToMap()
        {
            if (this.Map != null)
            {
                this.ID = this.Map.GetNewObjID;
                this.Map.Npcs.Add(this);
            }
        }

        /// <summary>
        /// Extra content :)
        /// </summary>
        public void Speak(string message)
        {
            if (this.Map != null)
            {
                this.Map.Send("cMK|" + this.ID + "|" + this.Positions.Template.Name + "|" + message);
            }
        }

        public string DisplayPattern
        {
            get
            {
                if (Positions.Template != null)
                {
                    return "|+" + this.Positions.CellID + ";" + this.Positions.Dir + ";0;" + this.ID + ";" + this.Positions.NpcID + ";-4;" + 
                        this.Positions.Template.Skin + "^" + this.Positions.Template.ScaleX + ";" + this.Positions.Template.Sex + ";"
                        + this.Positions.Template.Color1.ToString("x") + ";" + this.Positions.Template.Color2.ToString("x") + ";"
                        + this.Positions.Template.Color3.ToString("x") + ";" + this.Positions.Template.Accessories + ";;"
                        + this.Positions.Template.Artwork;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
