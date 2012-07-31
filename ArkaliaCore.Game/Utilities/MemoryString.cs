using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Utilities
{
    public class MemoryString
    {
        private string _value { get; set; }

        public MemoryString() { }

        public MemoryString(string value)
        {
            this._value = value;
        }

        public void Update(string value)
        {
            this._value = value;
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }
    }
}
