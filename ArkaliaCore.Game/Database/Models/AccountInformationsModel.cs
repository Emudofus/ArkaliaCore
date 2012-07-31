using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class AccountInformationsModel
    {
        public int ID { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string FriendsGuid { get; set; }

        public List<Game.Friends.Friend> Friends = new List<Game.Friends.Friend>();

        #region Friends Methods

        public string FriendsToDatabase
        {
            get
            {
                lock (Friends)
                {
                    return string.Join(",", Friends);
                }
            }
        }

        public bool HaveFriend(int id)
        {
            lock (Friends)
            {
                return this.Friends.FindAll(x => x.Account.AccountId == id).Count > 0;
            }
        }

        public Game.Friends.Friend GetFriendByNickname(string name)
        {
            lock (Friends)
            {
                if (this.Friends.FindAll(x => x.Account.Username.ToLower() == name.ToLower()).Count > 0)
                {
                    return this.Friends.FirstOrDefault(x => x.Account.Username.ToLower() == name.ToLower());
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        public void Save()
        {
            Database.Tables.AccountInformationsTable.Save(this);
        }
    }
}
