using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class WaypointModel
    {
        public int ID { get; set; }
        public int MapID { get; set; }
        public int CellID { get; set; }

        public MapModel Map
        {
            get
            {
                return Game.Controllers.MapController.GetMap(this.MapID);
            }
        }

        public override string ToString()
        {
            return this.MapID.ToString();
        }
    }
}
