using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ArkaliaCore.Tool.Kernel
{
    public class KernelFile
    {
        public string Path { get; set; }

        public List<MapFlag> MapFlags = new List<MapFlag>();

        public KernelFile() { }

        public KernelFile(string path)
        {
            this.Path = path;
        }

        public void Read()
        {

        }

        public void Save()
        {
            var writer = new BinaryWriter(new FileStream(this.Path, FileMode.CreateNew));
            writer.Write(this.MapFlags.Count);
            this.MapFlags.ForEach(x => x.Write(writer));
            writer.Close();
        }
    }
}
