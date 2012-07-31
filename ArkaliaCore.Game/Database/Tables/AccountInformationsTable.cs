using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class AccountInformationsTable
    {
        public static int NextGuid = 1;
        public static List<Models.AccountInformationsModel> Cache = new List<Models.AccountInformationsModel>();
        public static Utilities.GuidGenerator Guid = new Utilities.GuidGenerator();

        public const string Collums = "ID,AccountID,Nickname,FriendsGuids";

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @account informations@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM accounts_data");
            while (reader.Read())
            {
                try
                {
                    var account = new Models.AccountInformationsModel()
                    {
                        ID = reader.GetInt32("ID"),
                        AccountId = reader.GetInt32("AccountID"),
                        Username = reader.GetString("Nickname"),
                        FriendsGuid = reader.GetString("FriendsGuids"),
                    };

                    #region Friends

                    foreach (var friend in account.FriendsGuid.Split(','))
                    {
                        try
                        {
                            if (friend != "")
                            {
                                account.Friends.Add(new Game.Friends.Friend(int.Parse(friend)));
                            }
                        }
                        catch (Exception e)
                        {
                            Utilities.Logger.Error("Can't loade friends for account '" + account.Username + "' : " + e.Message);
                        }
                    }

                    #endregion

                    Guid.Update(account.ID);
                    Cache.Add(account);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't load account : " + e.Message);
                }
            }
            reader.Close();

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ account informations");
        }

        public static void Insert(Models.AccountInformationsModel account)
        {
            lock (DatabaseManager.Locker)
            {
                try
                {
                    var query = "'" + account.ID + "','" + account.AccountId + "','" + account.Username + "','" + account.FriendsGuid + "'";
                    DatabaseManager.Provider.ExecuteQuery("INSERT INTO accounts_data (" + Collums + ") VALUES (" + query + ")");
                    Guid.Update(account.ID);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't execute query : " + e.ToString());
                }
            }
        }

        public static void Save(Models.AccountInformationsModel account)
        {
            lock (DatabaseManager.Locker)
            {
                try
                {
                    string values = "ID = '" + account.ID + "', AccountID = '" + account.AccountId + "', Nickname = '" + account.Username + "', "
                        + "FriendsGuids = '" + account.FriendsToDatabase + "'";
                    string query = "UPDATE accounts_data SET " + values + " WHERE Id = '" + account.ID + "'";
                    DatabaseManager.Provider.ExecuteQuery(query);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't execute query : " + e.ToString());
                }
            }
        }
    }
}
