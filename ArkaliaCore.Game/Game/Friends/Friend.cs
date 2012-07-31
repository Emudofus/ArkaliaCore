using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Friends
{
    public class Friend
    {
        public int ID { get; set; }

        public Friend(int id)
        {
            this.ID = id;
        }

        public Database.Models.AccountInformationsModel Account
        {
            get
            {
                return Accounts.AccountManager.GetAccountInformations(this.ID);
            }
        }

        public bool IsFriendly(Models.AccountModel account)
        {
            lock (this.Account)
            {
                return this.Account.Friends.FindAll(x => x.Account.AccountId == account.Infos.AccountId).Count > 0;
            }
        }

        public override string ToString()
        {
            return this.ID.ToString();
        }
    }
}
