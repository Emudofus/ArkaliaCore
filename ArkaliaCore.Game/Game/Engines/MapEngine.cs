using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Engines
{
    public static class MapEngine
    {
        public static void AddPlayer(this Database.Models.MapModel map, Network.Game.GameClient client)
        {
            lock (map.Players)
            {
                if (!map.Players.Contains(client))
                {
                    map.Players.ForEach(x => ShowPlayer(map, x, client));
                    map.Players.Add(client);
                }
            }
        }

        public static void RemovePlayer(this Database.Models.MapModel map, Network.Game.GameClient client)
        {
            lock (map.Players)
            {
                if (map.Players.Contains(client))
                {
                    map.Players.Remove(client);
                    map.Players.ForEach(x => UnShowPlayer(map, x, client));
                }
            }
        }

        public static void ShowMap(this Database.Models.MapModel map, Network.Game.GameClient client)
        {
            if (map.DecryptKey != "")
            {
                client.Send(new StringBuilder("GDM|").Append(map.ID)
                    .Append("|").Append(map.Date).Append("|").Append(map.DecryptKey).ToString());
            }
            else
            {
                client.Send(new StringBuilder("GDM|").Append(map.ID)
                              .Append("|").Append(map.Date).ToString());
            }
        }

        public static void Send(this Database.Models.MapModel map, string packet)
        {
            lock (map.Players)
            {
                map.Players.ForEach(x => x.Send(packet));
            }
        }

        public static void ShowPlayer(this Database.Models.MapModel map, Network.Game.GameClient client, Network.Game.GameClient toShow)
        {
            StringBuilder pattern = new StringBuilder("GM|+");
            pattern.Append(toShow.Character.ShowPattern);
            client.Send(pattern.ToString());
        }

        public static void ShowPlayers(this Database.Models.MapModel map, Network.Game.GameClient client)
        {
            lock (map.Players)
            {
                StringBuilder pattern = new StringBuilder("GM");
                map.Players.ForEach(x => pattern.Append("|+").Append(x.Character.ShowPattern));
                client.Send(pattern.ToString());
            }
        }

        public static void ShowNpcs(this Database.Models.MapModel map, Network.Game.GameClient client)
        {
            lock (map.Npcs)
            {
                StringBuilder pattern = new StringBuilder("GM");
                map.Npcs.ForEach(x => pattern.Append(x.DisplayPattern));
                client.Send(pattern.ToString());
            }
        }

        public static void UnShowPlayer(this Database.Models.MapModel map, Network.Game.GameClient client, Network.Game.GameClient toUnShow)
        {
            StringBuilder pattern = new StringBuilder("GM|-");
            pattern.Append(toUnShow.Character.ID);
            client.Send(pattern.ToString());
        }
    }
}
