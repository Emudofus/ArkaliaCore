using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Interop.Crystal.Packets
{
    public class PlayerDeletedCharacterMessage : CrystalPacket
    {
        public PlayerDeletedCharacterMessage(string name)
            : base(PacketHeaderEnum.PlayerDeletedCharacterMessage)
        {
            Writer.Write(name);
        }
    }
}
