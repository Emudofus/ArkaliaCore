using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game
{
    public static class World
    {
        public static List<Network.Game.GameClient> GetLogged()
        {
            lock (Network.Game.GameServer.Clients)
            {
            return Network.Game.GameServer.Clients.FindAll(x => x.Character != null);
            }
        }

        public static void Send(string packet)
        {
            foreach (var client in GetLogged())
            {
                client.Send(packet);
            }
        }

        public static void SendAdmin(string packet)
        {
            foreach (var client in GetLogged().FindAll(x => x.Account.AdminLevel > 0))
            {
                client.Send(packet);
            }
        }

        public static Network.Game.GameClient GetClient(string nickname)
        {
            var connected = GetLogged();
            if (connected.FindAll(x => x.Character.Nickname.ToLower() == nickname.ToLower()).Count > 0)
            {
                return connected.FirstOrDefault(x => x.Character.Nickname.ToLower() == nickname.ToLower());
            }
            else
            {
                return null;
            }
        }

        public static Network.Game.GameClient GetClientByAccount(int id)
        {
            var connected = GetLogged();
            if (connected.FindAll(x => x.Account.ID == id).Count > 0)
            {
                return connected.FirstOrDefault(x => x.Account.ID == id);
            }
            else
            {
                return null;
            }
        }

        public static Network.Game.GameClient GetClientByPseudo(string nickname)
        {
            var connected = GetLogged();
            if (connected.FindAll(x => x.Account.Pseudo.ToLower() == nickname.ToLower()).Count > 0)
            {
                return connected.FirstOrDefault(x => x.Account.Pseudo.ToLower() == nickname.ToLower());
            }
            else
            {
                return null;
            }
        }
    }
}
