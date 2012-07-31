using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Database.Tables
{
    public static class AccountCharacterTable
    {
        public static List<Models.AccountCharacterModel> Cache = new List<Models.AccountCharacterModel>();
        public static string Collums = "ID,Server,Name,Owner";

        public static void Load()
        {
            Utilities.Logger.Infos("Load @Characters Informations@ ...");
            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM account_characters_informations");
            while (reader.Read())
            {
                var character = new Models.AccountCharacterModel()
                {
                    ID = reader.GetInt32("ID"),
                    Server = reader.GetInt32("Server"),
                    Name= reader.GetString("Name"),
                    Owner = reader.GetInt32("Owner"),
                };
                Cache.Add(character);
            }
            reader.Close();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "' Characters Informations@ from database");
        }

        public static void Insert(Models.AccountCharacterModel character)
        {
            lock (DatabaseManager.Locker)
            {
                string query = "INSERT INTO account_characters_informations  (" + Collums + ") VALUES (%1)";
                string values = "'" + character.ID + "', '" + character.Server + "', '" + character.Name + "', '" + character.Owner + "'";
                DatabaseManager.Provider.ExecuteQuery(query.Replace("%1", values));
            }
        }

        public static void Delete(Models.AccountCharacterModel character)
        {
            lock (DatabaseManager.Locker)
            {
                DatabaseManager.Provider.ExecuteQuery("DELETE FROM account_characters_informations WHERE Name='" + character.Name + "'");
            }
        }
    }
}
