using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Engines.Pathfinding
{
    public class PathNode
    {
        public int CellID { get; set; }
        public int Dir { get; set; }

        public PathNode(int cellid, int dir)
        {
            this.CellID = cellid;
            this.Dir = dir;
        }

        public override string ToString()
        {
            return PathEngine.CreateNodePath(this);
        }
    }
}
