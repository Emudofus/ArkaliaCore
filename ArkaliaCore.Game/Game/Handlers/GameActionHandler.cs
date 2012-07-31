using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class GameActionHandler
    {
        public static void HandleGameAction(Network.Game.GameClient client, string packet)
        {
            int Type = int.Parse(packet.Substring(2, 3));
            string parameters = packet.Substring(5);
            switch ((Enums.GameActionEnum)Type)
            {
                case Enums.GameActionEnum.MOVE:
                    RequestMove(client, parameters);
                    break;

                case Enums.GameActionEnum.USE_OBJECT:
                    RequestUseObject(client, parameters);
                    break;
            }
        }

        public static void RequestMove(Network.Game.GameClient client, string parameters)
        {
            if (!client.Character.IsOverpods)
            {
                var path = new Engines.PathEngine(parameters, client.Character.CellID, client.Character.Direction, client.Character.Map);
                string finalPath = path.Path;
                if (finalPath.Length > 0)
                {
                    var lastNode = path.LastNode();
                    if (lastNode != null)
                    {
                        client.EndAutoRegen();
                        Utilities.Logger.Debug("Pathfinding : " + finalPath + ", Last node : " + path.LastNode().CellID + "(" + path.LastNode().ToString() + ")");
                        client.CurrentMove = path;
                        client.Character.Map.Send(path.GetPathPattern(client.Character.ID));
                    }
                }
            }
            else
            {
                client.SendImPacket("112");
                client.Send("GA;0");
            }
        }

        public static void RequestUseObject(Network.Game.GameClient client, string parameters)
        {
            var data = parameters.Split(';');
            var cellid = int.Parse(data[0]);
            var typeID = (Enums.GameObjectTypeEnum)int.Parse(data[1]);
            Utilities.Logger.Debug("Request use object type " + typeID.ToString() + " on cell " + cellid);
            client.CurrentObjectUsed = typeID;
        }

        public static void RequestChangeMove(Network.Game.GameClient client, string parameters)
        {
            if (client.CurrentMove != null)
            {
                client.CurrentObjectUsed = Enums.GameObjectTypeEnum.NONE;
                client.Character.CellID = int.Parse(parameters.Substring(5));
                client.CurrentMove = null;
            }
        }

        public static void CheckTrigger(Network.Game.GameClient client)
        {
            var trigger = client.Character.Map.GetTrigger(client.Character.CellID);
            if (trigger != null)
            {
                client.Teleport(trigger.NextMap, trigger.NextCell);
            }
        }

        public static void RequestEndMove(Network.Game.GameClient client)
        {
            if (client.Character.Map != null)
            {
                if (client.CurrentMove != null)
                {
                    var lastNode = client.CurrentMove.LastNode();
                    client.Character.CellID = lastNode.CellID;
                    client.Character.Direction = lastNode.Dir;
                    client.CurrentMove = null;

                    if (client.CurrentObjectUsed != Enums.GameObjectTypeEnum.NONE)
                    {
                        switch (client.CurrentObjectUsed)
                        {
                            case Enums.GameObjectTypeEnum.ZAAP:
                                WaypointHandler.RequestWaypointList(client, client.Character.CellID);
                                break;

                            case Enums.GameObjectTypeEnum.SAVE_POSITION:
                                client.SavePosition();
                                break;
                        }
                        client.CurrentObjectUsed = Enums.GameObjectTypeEnum.NONE;
                    }

                    Modules.Scripting.ScriptKernel.HandlePlayerEvents(client, "finish_move",
                        new KeyValuePair<string, object>("mapid", client.Character.MapID),
                        new KeyValuePair<string, object>("cellid", client.Character.CellID));

                    CheckTrigger(client);
                }
            }
        }
    }
}
