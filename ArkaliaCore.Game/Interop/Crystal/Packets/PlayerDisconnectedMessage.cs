using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Interop.Crystal.Packets
{
    public class PlayerDisconnectedMessage : CrystalPacket
    {
        public PlayerDisconnectedMessage(string name)
            : base(PacketHeaderEnum.PlayerDisconnectedMessage)
        {
            Writer.Write(name);
        }
    }
}
