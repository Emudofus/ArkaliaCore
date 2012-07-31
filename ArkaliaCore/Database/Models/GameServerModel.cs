using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Database.Models
{
    public class GameServerModel
    {
        public int ID { get; set; }
        public string Adress { get; set; }
        public int GamePort { get; set; }
        public int CommunicationPort { get; set; }

        public Enums.ServerStateEnum State = Enums.ServerStateEnum.Offline;
    }
}
