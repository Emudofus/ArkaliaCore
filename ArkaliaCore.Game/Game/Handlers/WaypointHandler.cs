using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class WaypointHandler
    {
        public static void RequestWaypointList(Network.Game.GameClient client, int cellid)
        {
            if (client.Character.Map != null)
            {
                var from = client.Character.Map;
                var waypoint = Waypoints.WaypointManager.GetWaypoint(from.ID);

                if (waypoint != null)
                {
                    if (!client.Character.HaveWaypoint(waypoint.MapID, waypoint.CellID))
                    {
                        client.Character.Waypoints.Add(waypoint);
                        client.Character.Save();
                        client.Send("Im024;");
                    }

                    SendWaypointsList(client);
                }
                else
                {
                    client.ErrorMessage("Impossible de traiter la demande de zaap !");
                }
            }
        }

        public static void HandleUseWaypoint(Network.Game.GameClient client, string packet)
        {
            var mapid = int.Parse(packet.Substring(2));
            var from = Waypoints.WaypointManager.GetWaypoint(client.Character.MapID);
            var to = Waypoints.WaypointManager.GetWaypoint(mapid);

            if (from != null && to != null)
            {
                var cost = Utilities.Formulas.CalculateWaypointCost(from.Map, to.Map);
                if (client.Character.Kamas >= cost)
                {
                    client.Character.Kamas -= cost;
                    client.SendStats();
                    CloseWaypoint(client);
                    client.Teleport(to.MapID, to.CellID);
                }
            }
        }

        public static void HandleCloseWaypoint(Network.Game.GameClient client, string packet)
        {
            CloseWaypoint(client);
        }

        public static void CloseWaypoint(Network.Game.GameClient client)
        {
            client.Send("WV");
        }

        public static void SendWaypointsList(Network.Game.GameClient client)
        {
            var pattern = new StringBuilder("WC" + client.Character.SaveMap + "|");
            client.Character.Waypoints.ForEach(x => pattern.Append(x.MapID + ";" + 
                Utilities.Formulas.CalculateWaypointCost(client.Character.Map, x.Map) + "|"));
            client.Send(pattern.ToString().Substring(0, pattern.ToString().Length - 1));
        }
    }
}
