using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Breeds
{
    public static class BreedManager
    {
        public static Database.Models.BreedModel GetBreed(int id)
        {
            lock (Database.Tables.BreedTable.Cache)
            {
                return Database.Tables.BreedTable.Cache[id];
            }
        }
    }
}
