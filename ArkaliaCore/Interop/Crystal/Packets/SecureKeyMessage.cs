using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Interop.Crystal.Packets
{
    public class SecureKeyMessage : CrystalPacket
    {
        public SecureKeyMessage(string key, string name)
            : base(PacketHeaderEnum.SecureKeyMessage)
        {
            Writer.Write(key);
            Writer.Write(name);
        }
    }
}
