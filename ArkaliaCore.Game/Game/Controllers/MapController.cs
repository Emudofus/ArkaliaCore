using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Controllers
{
    public static class MapController
    {
        public static Database.Models.MapModel GetMap(int id)
        {
            lock (Database.Tables.MapTable.Cache)
            {
                if (Database.Tables.MapTable.Cache.ContainsKey(id))
                {
                    return Database.Tables.MapTable.Cache[id];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
