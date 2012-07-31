using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class NpcModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Skin { get; set; }
        public int ScaleX { get; set; }
        public int ScaleY { get; set; }
        public int Sex { get; set; }
        public int Color1 { get; set; }
        public int Color2 { get; set; }
        public int Color3 { get; set; }
        public string Accessories { get; set; }
        public int Clip { get; set; }
        public int Artwork { get; set; }
        public int Bonus { get; set; }
        public int InitQuestion { get; set; }
        public string SaleItems { get; set; }

        public NpcDialogModel Dialog { get; set; }
    }
}
