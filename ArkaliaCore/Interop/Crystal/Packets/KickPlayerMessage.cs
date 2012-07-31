using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Interop.Crystal.Packets
{
    public class KickPlayerMessage : CrystalPacket
    {
        public KickPlayerMessage(string account)
            : base(PacketHeaderEnum.KickPlayerMessage)
        {
            Writer.Write(account);
        }
    }
}
