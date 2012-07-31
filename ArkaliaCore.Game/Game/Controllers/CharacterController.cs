using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Controllers
{
    public static class CharacterController
    {
        public static List<string> NotAllowedCharInNickname = new List<string>()
        { "@", "<", "_", "1", "2", "3", "4", "5", "6", "7", "8", "9", "/", "*", "[", "]", "(", ")" };

        public static List<Database.Models.CharacterModel> GetCharacterForOwner(int owner)
        {
            lock (Database.Tables.CharacterTable.Cache)
            {
                return Database.Tables.CharacterTable.Cache.Values.ToList().FindAll(x => x.Owner == owner);
            }
        }

        public static bool ValidNickname(string nickname)
        {
            lock (NotAllowedCharInNickname)
            {
                foreach (var c in NotAllowedCharInNickname)
                {
                    if (nickname.Contains(c))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int AvailableID()
        {
            lock (Database.Tables.CharacterTable.Cache)
            {
                var value = 1;
                while (Database.Tables.CharacterTable.Cache.ContainsKey(value))
                {
                    value++;
                }
                return value;
            }
        }

        public static bool ExistCharacter(string nickname)
        {
            lock (Database.Tables.CharacterTable.Cache)
            {
                if (Database.Tables.CharacterTable.Cache.Values.ToList().FindAll(x => x.Nickname.ToLower() == nickname.ToLower()).Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static Database.Models.CharacterModel GetCharacter(int id)
        {
            lock (Database.Tables.CharacterTable.Cache)
            {
                if (Database.Tables.CharacterTable.Cache.ContainsKey(id))
                {
                    return Database.Tables.CharacterTable.Cache[id];
                }
                else
                {
                    return null;
                }
            }
        }

        public static int BaseLook(Database.Models.CharacterModel character)
        {
            return int.Parse(character.Breed.ToString() + character.Gender.ToString());
        }
    }
}
