using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class TriggerModel
    {
        public int ID { get; set; }
        public int MapID { get; set; }
        public int CellID { get; set; }
        public int NextMap { get; set; }
        public int NextCell { get; set; }
    }
}
