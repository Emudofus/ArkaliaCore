using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class WorldItemTable
    {
        public static Dictionary<int, Models.WorldItemModel> Cache = new Dictionary<int, Models.WorldItemModel>();
        public static string Collums = "Id,Owner,Template,Position,Quantity,Effects";

        #region TempID

        private static int _tempID = 1;
        public static int TempID
        {
            get
            {
                _tempID++;
                return _tempID;
            }
        }

        #endregion

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @players items@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM world_items");
            while (reader.Read())
            {
                try
                {
                    var item = new Models.WorldItemModel()
                    {
                        ID = reader.GetInt32("Id"),
                        Owner = reader.GetInt32("Owner"),
                        TemplateID = reader.GetInt32("Template"),
                        Position = reader.GetInt32("Position"),
                        Quantity = reader.GetInt32("Quantity"),
                        Effects = reader.GetString("Effects"),
                    };
                    item.Engine = new Game.Engines.EffectEngine(item.Effects);
                    item.Engine.Load();
                    Cache.Add(item.ID, item);

                    //Update ID
                    if (item.ID > _tempID)
                    {
                        _tempID = item.ID + 1;
                    }

                    //Assign item to character
                    var owner = Game.Controllers.CharacterController.GetCharacter(item.Owner);
                    if (owner != null)
                    {
                        owner.Bag.Add(item);
                    }
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Failed to load item : " + e.ToString());
                }
            }
            reader.Close();

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ player items");
        }

        public static void Insert(Models.WorldItemModel item)
        {
            try
            {
                string values = "'" + item.ID + "', '" + item.Owner + "', '" + item.TemplateID + "', '" + item.Position + "', " + 
                                "'" + item.Quantity + "', '" + item.Effects + "'";

                string query = "INSERT INTO world_items (" + Collums + ") VALUES ({0})";

                lock (DatabaseManager.Locker)
                    DatabaseManager.Provider.ExecuteQuery(query.Replace("{0}", values));

                lock (Cache)
                    if (!Cache.ContainsKey(item.ID))
                    {
                        Cache.Add(item.ID, item);
                    }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't execute query : " + e.ToString());
            }
        }

        public static void Save(Models.WorldItemModel item)
        {
            try
            {
                string values = "Id = '" + item.ID + "', Owner = '" + item.Owner + "', Template = '" + item.TemplateID + "', " + 
                    "Position = '" + item.Position + "', Quantity = '" + item.Quantity + "', Effects = '" + item.Effects + "'";

                string query = "UPDATE world_items SET " + values + " WHERE Id = '" + item.ID + "'";

                lock (DatabaseManager.Locker)
                    DatabaseManager.Provider.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't execute query : " + e.ToString());
            }
        }

        public static void Delete(Models.WorldItemModel item)
        {
            lock (DatabaseManager.Locker)
            {
                try
                {
                    lock (Cache)
                        if (Cache.ContainsKey(item.ID))
                        {
                            Cache.Remove(item.ID);
                        }
                    DatabaseManager.Provider.ExecuteQuery("DELETE FROM world_items WHERE ID = '" + item.ID + "'");
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't execute query : " + e.ToString());
                }
            }
        }
    }
}
