using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Waypoints
{
    public static class WaypointManager
    {
        public static Database.Models.WaypointModel GetWaypoint(int mapid)
        {
            lock (Database.Tables.WaypointTable.Cache)
            {
                if (Database.Tables.WaypointTable.Cache.FindAll(x => x.MapID == mapid).Count > 0)
                {
                    return Database.Tables.WaypointTable.Cache.FirstOrDefault(x => x.MapID == mapid);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
