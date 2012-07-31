using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Interop.Crystal.Packets
{
    public class PlayerCreatedCharacterMessage : CrystalPacket
    {
        public PlayerCreatedCharacterMessage(Database.Models.CharacterModel character)
            : base(PacketHeaderEnum.PlayerCreatedCharacterMessage)
        {
            Writer.Write(character.Owner);
            Writer.Write(character.Nickname);
        }
    }
}
