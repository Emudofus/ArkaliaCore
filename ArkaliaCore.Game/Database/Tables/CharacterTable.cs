using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class CharacterTable
    {
        public static Dictionary<int, Models.CharacterModel> Cache = new Dictionary<int, Models.CharacterModel>();
        public static string Collums = 
            "ID,Owner,Nickname,Level,Experience,Look,Scal,Gender,Breed,Color1,Color2,Color3," + 
            "MapID,CellID,Direction,Kamas,CurrentLife,Life,Wisdom,Strenght,Fire,Agility,Water,CaractPoint,SpellPoint," +
            "SaveMap,SaveCell,OwnZaaps,OwnSpells,FactionID,FactionPower,FactionHonor,FactionDeshonor,FactionEnabled," + 
            "GuildID,GuildRank,GuildRights,MountID,RideMount,TitleID,EliteLevel,Emotes,Jobs";

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @characters@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();
            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM characters");
            while (reader.Read())
            {
                var character = new Models.CharacterModel()
                {
                    ID = reader.GetInt32("ID"),
                    Owner = reader.GetInt32("Owner"),
                    Nickname = reader.GetString("Nickname"),
                    Level = reader.GetInt32("Level"),
                    Experience = reader.GetInt64("Experience"),
                    Look = reader.GetInt32("Look"),
                    Scal = reader.GetInt32("Scal"),
                    Gender = reader.GetInt32("Gender"),
                    Breed = reader.GetInt32("Breed"),
                    Color1 = reader.GetInt32("Color1"),
                    Color2 = reader.GetInt32("Color2"),
                    Color3 = reader.GetInt32("Color3"),
                    MapID = reader.GetInt32("MapID"),
                    CellID = reader.GetInt32("CellID"),
                    Direction = reader.GetInt32("Direction"),
                    Kamas = reader.GetInt32("Kamas"),
                    CurrentLife = reader.GetInt32("CurrentLife"),
                    Life = reader.GetInt32("Life"),
                    Wisdom = reader.GetInt32("Wisdom"),
                    Strenght = reader.GetInt32("Strenght"),
                    Fire = reader.GetInt32("Fire"),
                    Agility = reader.GetInt32("Agility"),
                    Water = reader.GetInt32("Water"),
                    CaractPoint = reader.GetInt32("CaractPoint"),
                    SpellPoint = reader.GetInt32("SpellPoint"),
                    SaveMap = reader.GetInt32("SaveMap"),
                    SaveCell = reader.GetInt32("SaveCell"),
                    OwnZaaps = reader.GetString("OwnZaaps"),
                    OwnSpells = reader.GetString("OwnSpells"),
                    EmoteList = reader.GetString("Emotes"),
                    JobsList = reader.GetString("Jobs"),
                    //TODO: Complete Fields
                };

                #region Emotes

                //Load emotes
                foreach (var emote in character.EmoteList.Split(',')) { if (emote != "") { character.Emotes.Add(int.Parse(emote)); } }

                #endregion

                #region Jobs

                foreach (var job in character.JobsList.Split(';'))
                {
                    if (job != "")
                    {
                        var data = job.Split(',');
                        var jobInstance = new Game.Jobs.JobInstance(Game.Jobs.JobsManager.GetJob(int.Parse(data[0])), int.Parse(data[1]))
                        {
                            Exp = int.Parse(data[2]),
                        };
                        character.Jobs.Add(jobInstance);
                    }
                }

                #endregion

                #region Waypoints

                foreach (var w in character.OwnZaaps.Split(','))
                {
                    if (w != "")
                    {
                        var waypoint = Game.Waypoints.WaypointManager.GetWaypoint(int.Parse(w));
                        if (waypoint != null)
                        {
                            character.Waypoints.Add(waypoint);
                        }
                    }
                }

                #endregion

                character.UpdateStatsFields();
                Cache.Add(character.ID, character);
            }
            reader.Close();
            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ characters");
        }

        public static void Insert(Models.CharacterModel character)
        {
            try
            {
                string values = "'" + character.ID + "', '" + character.Owner + "',";
                values += "'" + character.Nickname + "', '" + character.Level + "',";
                values += "'" + character.Experience + "', '" + character.Look + "',";
                values += "'" + character.Scal + "', '" + character.Gender + "',";
                values += "'" + character.Breed + "', '" + character.Color1 + "',";
                values += "'" + character.Color2 + "', '" + character.Color3 + "',";
                values += "'" + character.MapID + "', '" + character.CellID + "',";
                values += "'" + character.Direction + "', '" + character.Kamas + "',";
                values += "'" + character.CurrentLife + "', '" + character.Life + "',";
                values += "'" + character.Wisdom + "', '" + character.Strenght + "',";
                values += "'" + character.Fire + "', '" + character.Agility + "',";
                values += "'" + character.Water + "', '" + character.CaractPoint + "',";
                values += "'" + character.SpellPoint + "', '" + character.SaveMap + "',";
                values += "'" + character.SaveCell + "', '" + character.OwnZaaps + "',";
                values += "'" + character.OwnSpells + "', '" + character.FactionID + "',";
                values += "'" + character.FactionPower + "', '" + character.FactionHonor + "',";
                values += "'" + character.FactionDeshonor + "', '" + character.FactionEnabled + "',";
                values += "'" + character.GuildID + "', '" + character.GuildRank + "',";
                values += "'" + character.GuildRights + "', '" + character.MountID + "',";
                values += "'" + character.RideMount + "', '" + character.TitleID + "',";
                values += "'" + character.EliteLevel + "', '" + character.EmotesString + "', '" + character.JobsString + "'";

                string query = "INSERT INTO characters (" + Collums + ") VALUES ({0})";

                lock(DatabaseManager.Locker)
                    DatabaseManager.Provider.ExecuteQuery(query.Replace("{0}", values));
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't execute query : " + e.ToString());
            }

        }

        public static void Delete(Models.CharacterModel character)
        {
            lock (DatabaseManager.Locker)
            {
                try
                {
                    lock(Cache)
                        if (Cache.ContainsKey(character.ID))
                        {
                            Cache.Remove(character.ID);
                        }
                    DatabaseManager.Provider.ExecuteQuery("DELETE FROM characters WHERE ID = '" + character.ID + "'");
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't execute query : " + e.ToString());
                }
            }
        }

        public static void Save(Models.CharacterModel character)
        {
            try
            {
                string values = "Owner = '" + character.Owner + "', Nickname = '" + character.Nickname + "', Level = '" + character.Level + "', " +
                    "Experience = '" + character.Experience + "', Look = '" + character.Look + "', Scal = '" + character.Scal + "', " +
                    "Gender = '" + character.Gender + "', Breed = '" + character.Breed + "', Color1 = '" + character.Color1 + "', Color2 = '" + character.Color2 + "', " +
                    "Color3 = '" + character.Color3 + "', MapID = '" + character.MapID + "', CellID = '" + character.CellID + "', " +
                    "Direction = '" + character.Direction + "', Kamas = '" + character.Kamas + "', CurrentLife = '" + character.CurrentLife + "', " +
                    "Life = '" + character.Life + "', Wisdom = '" + character.Wisdom + "', Strenght = '" + character.Strenght + "', " +
                    "Fire = '" + character.Fire + "', Agility = '" + character.Agility + "', Water = '" + character.Water + "', " +
                    "CaractPoint = '" + character.CaractPoint + "', SpellPoint = '" + character.SpellPoint + "', SaveMap = '" + character.SaveMap + "', " +
                    "SaveCell = '" + character.SaveCell + "', OwnZaaps = '" + character.WaypointString + "', Emotes = '" + character.EmotesString + "', Jobs = '" + character.JobsString + "'";
                string query = "UPDATE characters SET " + values + " WHERE Id = '" + character.ID + "'";

                lock (DatabaseManager.Locker)
                    DatabaseManager.Provider.ExecuteQuery(query);

                Utilities.Logger.Debug("Character @'" + character.Nickname + "'@ saved");
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't execute query : " + e.ToString());
            }
        }
    }
}
