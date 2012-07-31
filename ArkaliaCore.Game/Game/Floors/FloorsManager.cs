using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Database.Tables;

namespace ArkaliaCore.Game.Game.Floors
{
    public static class FloorsManager
    {
        public static Database.Models.ExpFloorModel GetFloorByLevel(int level)
        {
            lock (ExpFloorTable.Cache)
            {
                if (ExpFloorTable.Cache.ContainsKey(level))
                {
                    return ExpFloorTable.Cache[level];
                }
                else
                {
                    return null;
                }
            }
        }

        public static Database.Models.ExpFloorModel GetFloorByCharacterExp(int exp)
        {
            lock (ExpFloorTable.Cache)
            {
                Database.Models.ExpFloorModel floor = null;
                foreach (var f in ExpFloorTable.Cache.Values)
                {
                    if (f.Character <= exp)
                    {
                        floor = f;
                    }
                }
                return floor;
            }
        }

        public static Database.Models.ExpFloorModel GetNextFloor(int level)
        {
            lock (ExpFloorTable.Cache)
            {
                if (ExpFloorTable.Cache.ContainsKey(level + 1))
                {
                    return ExpFloorTable.Cache[level + 1];
                }
                else
                {
                    return null;
                }
            }
        }

        public static Database.Models.ExpFloorModel GetFloorByJobExp(int exp)
        {
            lock (ExpFloorTable.Cache)
            {
                Database.Models.ExpFloorModel floor = null;
                foreach (var f in ExpFloorTable.Cache.Values)
                {
                    if (f.Job <= exp)
                    {
                        floor = f;
                    }
                }
                return floor;
            }
        }
    }
}
