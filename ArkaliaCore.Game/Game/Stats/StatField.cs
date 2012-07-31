using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Stats
{
    public class StatField
    {
        public int Basic { get; set; }
        public int Items { get; set; }
        public int Buffs { get; set; }
        public int Mount { get; set; }
        public int Bonus { get; set; }

        public int Total
        {
            get
            {
                return Basic + Items + Buffs + Mount + Bonus;
            }
        }

        public void Clear()
        {
            Basic = 0;
            Items = 0;
            Buffs = 0;
            Mount = 0;
            Bonus = 0;
        }

        public override string ToString()
        {
            return Basic + "," + Items + "," + Bonus + "," + Mount;
        }
    }
}
