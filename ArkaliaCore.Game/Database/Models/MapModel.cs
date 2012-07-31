using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class MapModel
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Places { get; set; }
        public string DecryptKey { get; set; }
        public string MapData { get; set; }
        public string Cells { get; set; }
        public string Monsters { get; set; }
        public int Capabilities { get; set; }

        public string Mappos
        {
            get
            {
                return X + "," + Y + ",0";
            }
            set
            {
                var data = value.Split(',');
                X = int.Parse(data[0]);
                Y = int.Parse(data[1]);
            }
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int NumGroup { get; set; }
        public int GroupMaxSize { get; set; }
        public string SpawnEmitters { get; set; }

        public List<Network.Game.GameClient> Players = new List<Network.Game.GameClient>();
        public Dictionary<int, TriggerModel> Triggers = new Dictionary<int, TriggerModel>();
        public List<Game.Npcs.NpcInstance> Npcs = new List<Game.Npcs.NpcInstance>();

        private int _objMapID = -1;

        public int GetNewObjID
        {
            get
            {
                this._objMapID--;
                return this._objMapID;
            }
        }

        public Game.Npcs.NpcInstance GetNpc(int id)
        {
            lock (this.Npcs)
            {
                if (this.Npcs.FindAll(x => x.ID == id).Count > 0)
                {
                    return this.Npcs.FirstOrDefault(x => x.ID == id);
                }
                else
                {
                    return null;
                }
            }
        }

        public Game.Npcs.NpcInstance GetNpcByTemplate(int id)
        {
            lock (this.Npcs)
            {
                if (this.Npcs.FindAll(x => x.Positions.Template.ID == id).Count > 0)
                {
                    return this.Npcs.FirstOrDefault(x => x.Positions.Template.ID == id);
                }
                else
                {
                    return null;
                }
            }
        }

        public TriggerModel GetTrigger(int cellid)
        {
            lock (this.Triggers)
            {
                if (this.Triggers.ContainsKey(cellid))
                {
                    return Triggers[cellid];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
