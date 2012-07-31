using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Accounts
{
    public static class AccountManager
    {
        public static bool HaveAccountInformations(string username)
        {
            lock (Database.Tables.AccountInformationsTable.Cache)
            {
                return Database.Tables.AccountInformationsTable.Cache.FindAll(x => x.Username.ToLower() == username).Count > 0;
            }
        }

        public static void CreateAccountInformations(Models.AccountModel account)
        {
            lock (Database.Tables.AccountInformationsTable.Cache)
            {
                var infos = new Database.Models.AccountInformationsModel()
                {
                    ID = Database.Tables.AccountInformationsTable.Guid.Value,
                    AccountId = account.ID,
                    Username = account.Username,
                    FriendsGuid = "",
                };
                Database.Tables.AccountInformationsTable.Cache.Add(infos);
                Database.Tables.AccountInformationsTable.Insert(infos);
            }
        }

        public static Database.Models.AccountInformationsModel GetAccountInformations(string username)
        {
            lock (Database.Tables.AccountInformationsTable.Cache)
            {
                return Database.Tables.AccountInformationsTable.Cache.FirstOrDefault(x => x.Username.ToLower() == username);
            }
        }

        public static Database.Models.AccountInformationsModel GetAccountInformations(int id)
        {
            lock (Database.Tables.AccountInformationsTable.Cache)
            {
                return Database.Tables.AccountInformationsTable.Cache.FirstOrDefault(x => x.AccountId == id);
            }
        }
    }
}
