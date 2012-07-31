using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class NpcPosModel
    {
        public int ID { get; set; }
        public int NpcID { get; set; }
        public int MapID { get; set; }
        public int CellID { get; set; }
        public int Dir { get; set; }

        private NpcModel _template { get; set; }

        public MapModel Map
        {
            get
            {
                return Game.Controllers.MapController.GetMap(this.MapID);
            }
        }

        public NpcModel Template
        {
            get
            {
                if (this._template == null)
                {
                    this._template = Game.Npcs.NpcManager.GetNpc(this.NpcID);
                }
                return this._template;
            }
        }
    }
}
