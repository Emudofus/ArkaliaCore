using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class NpcDialogModel
    {
        public int ID { get; set; }
        public string ResponsesString { get; set; }
        public string Args { get; set; }

        public List<int> Responses = new List<int>();
    }
}
