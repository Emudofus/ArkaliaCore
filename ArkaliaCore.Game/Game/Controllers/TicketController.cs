using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Controllers
{
    public static class TicketController
    {
        public static Dictionary<string, Models.AccountTicket> Tickets = new Dictionary<string, Models.AccountTicket>();

        public static void RegisterTicket(Models.AccountTicket ticket)
        {
            ticket.ExpireTime = Environment.TickCount;

            lock (Tickets)
                Tickets.Add(ticket.Ticket, ticket);

            Utilities.Logger.Debug("Registered new ticket for account @'" + ticket.Account.Username + "'@");
        }

        public static Models.AccountTicket GetTicket(string ticket)
        {
            lock (Tickets)
            {
                if (Tickets.ContainsKey(ticket))
                {
                    return Tickets[ticket];
                }
                else
                {
                    return null;
                }
            }
        }

        public static void DestroyTicket(Models.AccountTicket ticket)
        {
            lock (Tickets)
            {
                if (Tickets.ContainsKey(ticket.Ticket))
                {
                    Tickets.Remove(ticket.Ticket);
                    Utilities.Logger.Debug("Destroyed ticket for account @'" + ticket.Account.Username + "'@");
                }
            }
        }
    }
}
