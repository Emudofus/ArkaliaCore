using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Models
{
    public class AccountTicket
    {
        public string Ticket { get; set; }
        public int ExpireTime { get; set; }
        public AccountModel Account { get; set; }
    }
}
