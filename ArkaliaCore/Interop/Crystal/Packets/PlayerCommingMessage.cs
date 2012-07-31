using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Interop.Crystal.Packets
{
    public class PlayerCommingMessage : CrystalPacket
    {
        public PlayerCommingMessage(Database.Models.AccountModel account, string ticket)
            : base(PacketHeaderEnum.PlayerCommingMessage)
        {
            Writer.Write(ticket);
            Writer.Write(account.ID);
            Writer.Write(account.Username);
            Writer.Write(account.Password);
            Writer.Write(account.Pseudo);
            Writer.Write("DELETE?");
            Writer.Write("ok");
            Writer.Write(account.AdminLevel);
            Writer.Write(account.Points);
            Writer.Write(account.Vip);
        }
    }
}
