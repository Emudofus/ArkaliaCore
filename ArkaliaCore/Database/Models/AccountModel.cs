using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Database.Models
{
    public class AccountModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Pseudo { get; set; }
        public int AdminLevel { get; set; }
        public int Points { get; set; }
        public int Vip { get; set; }
    }
}
