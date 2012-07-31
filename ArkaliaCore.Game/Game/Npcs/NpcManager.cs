using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Npcs
{
    public static class NpcManager
    {
        public static Database.Models.NpcModel GetNpc(int id)
        {
            lock (Database.Tables.NpcTable.Cache)
            {
                if (Database.Tables.NpcTable.Cache.ContainsKey(id))
                {
                    return Database.Tables.NpcTable.Cache[id];
                }
                else
                {
                    return null;
                }
            }
        }

        public static Database.Models.NpcModel GetNpcForDialogID(int id)
        {
            lock (Database.Tables.NpcTable.Cache)
            {
                if (Database.Tables.NpcTable.Cache.Values.ToList().FindAll(x => x.InitQuestion == id).Count > 0)
                {
                    return Database.Tables.NpcTable.Cache.Values.ToList().FirstOrDefault(x => x.InitQuestion == id);
                }
                else
                {
                    return null;
                }
            }
        }

        public static Database.Models.NpcDialogModel GetDialog(int id)
        {
            lock (Database.Tables.NpcDialogTable.Cache)
            {
                if (Database.Tables.NpcDialogTable.Cache.ContainsKey(id))
                {
                    return Database.Tables.NpcDialogTable.Cache[id];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
