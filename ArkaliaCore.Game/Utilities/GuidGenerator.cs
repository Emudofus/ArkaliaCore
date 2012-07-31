using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Utilities
{
    public class GuidGenerator
    {
        public int Value { get; set; }

        public void Update(int value)
        {
            if (value > this.Value)
            {
                this.Value = value + 1;
            }
        }
    }
}
