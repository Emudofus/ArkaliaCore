using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ArkaliaCore.Game.Utilities
{
    public class FileBuffer
    {
        public string Path { get; set; }
        public StreamWriter Writer { get; set; }

        public FileBuffer(string path)
        {
            this.Path = path;
            this.Load();
        }

        public void Load()
        {
            var bufferedString = "";
            if (File.Exists(this.Path))
            {
                var reader = new StreamReader(this.Path);
                bufferedString = reader.ReadToEnd();
                reader.Close();
            }
            this.Writer = new StreamWriter(this.Path);
            this.Writer.AutoFlush = true;
            this.Writer.Flush();
            this.Writer.Write(bufferedString);
        }

        public void WriteLine(string value)
        {
            lock (Writer)
            {
                this.Writer.WriteLine(value);
            }
        }
    }
}
