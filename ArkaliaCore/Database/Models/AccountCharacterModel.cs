using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Database.Models
{
    public class AccountCharacterModel
    {
        public int ID { get; set; }
        public int Server { get; set; }
        public string Name { get; set; }
        public int Owner { get; set; }
    }
}
