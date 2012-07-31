using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ArkaliaCore.Realm.Interop.Crystal
{
    public class CrystalPacket
    {
        public PacketHeaderEnum ID;
        public BinaryWriter Writer;
        public BinaryReader Reader;
        public MemoryStream Stream;

        public CrystalPacket(PacketHeaderEnum id)
        {
            try
            {
                ID = id;
                Stream = new MemoryStream();
                Writer = new BinaryWriter(Stream);
                Writer.Write((byte)id);
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't write crystal packet : " + e.ToString());
            }
        }

        public CrystalPacket(byte[] data)
        {
            try
            {
                Stream = new MemoryStream(data);
                Reader = new BinaryReader(Stream);
                ID = (PacketHeaderEnum)Reader.ReadByte();
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't read crystal packet : " + e.ToString());
            }
        }

        public byte[] GetBytes
        {
            get
            {
                return Stream.ToArray();
            }
        }
    }
}
