using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Database.Tables
{
    public static class AccountTable
    {
        public static Models.AccountModel GetAccountFromSQL(string username)
        {
            lock (DatabaseManager.Locker)
            {
                Models.AccountModel account = null;
                var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM accounts WHERE username='" + username + "'");
                if (reader.Read())
                {
                    account = new Models.AccountModel()
                    {
                        ID = reader.GetInt32("Id"),
                        Username = reader.GetString("Username"),
                        Password = reader.GetString("Password"),
                        Pseudo = reader.GetString("Pseudo"),
                        AdminLevel = reader.GetInt32("AdminLevel"),
                        Points = reader.GetInt32("Points"),
                        Vip = reader.GetInt32("Vip"),
                    };
                }
                reader.Close();
                return account;
            }
        }

        public static void UpdatePoints(string username, int points)
        {
            try
            {
                lock (DatabaseManager.Locker)
                {
                    Database.DatabaseManager.Provider.ExecuteQuery("UPDATE accounts SET Points='" + points + "' WHERE username='" + username + "'");
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't execute query : " + e.ToString());
            }
        }

        public static void UpdateLogged(string username, int logged)
        {
            try
            {
                lock (DatabaseManager.Locker)
                {
                    Database.DatabaseManager.Provider.ExecuteQuery("UPDATE accounts SET Logged='" + logged + "' WHERE username='" + username + "'");
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't execute query : " + e.ToString());
            }
        }
    }
}
