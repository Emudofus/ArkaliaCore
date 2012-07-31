using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Interop.Crystal.Packets
{
    public class PlayerConnectedMessage : CrystalPacket
    {
        public PlayerConnectedMessage(string name)
            : base(PacketHeaderEnum.PlayerConnectedMessage)
        {
            Writer.Write(name);
        }
    }
}
