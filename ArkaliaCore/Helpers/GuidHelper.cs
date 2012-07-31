using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Helpers
{
    public static class GuidHelper
    {
        public static int GetAvailableCharacterID()
        {
            lock (Database.Tables.AccountCharacterTable.Cache)
            {
                int id = 1;
                while (Database.Tables.AccountCharacterTable.Cache.FindAll(x => x.ID == id).Count > 0)
                {
                    id++;
                }
                return id;
            }
        }
    }
}
