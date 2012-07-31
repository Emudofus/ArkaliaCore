using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Interop.Crystal.Packets
{
    public class HelloKeyMessage : CrystalPacket
    {
        public HelloKeyMessage()
            : base(PacketHeaderEnum.HelloKeyMessage) { }
    }
}
