using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ArkaliaCore.Tool.Kernel
{
    public class MapFlag
    {
        public string Prefix { get; set; }

        public int MapID { get; set; }
        public int CellID { get; set; }
        public int Level { get; set; }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.Prefix);
        }

        public void Read(BinaryReader reader)
        {
            this.Prefix = reader.ReadString();
        }
    }
}
