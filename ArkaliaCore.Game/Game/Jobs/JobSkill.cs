using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Jobs
{
    public class JobSkill
    {
        public virtual int ID { get; set; }
        public virtual int Time { get; set; }
        public virtual int Min { get; set; }
        public virtual int Max { get; set; }
        public virtual int Chance { get; set; }
        public virtual bool CraftSkill { get; set; }
    }
}
