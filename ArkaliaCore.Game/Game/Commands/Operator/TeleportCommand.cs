using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Operator
{
    public class TeleportCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "teleport";
            }
        }

        public override int AccessLevel
        {
            get
            {
                return 1;
            }
        }

        public override string Description
        {
            get
            {
                return "Vous teleporte ou teleporte un joueur";
            }
        }

        public override bool NeedLoaded
        {
            get
            {
                return true;
            }
        }

        public override void Execute(Network.Game.GameClient client, CommandParameters parameters)
        {
            var mapid = parameters.GetIntParameter(0);
            var cellid = parameters.GetIntParameter(1);
            var toTeleport = client;

            if (parameters.Lenght > 2)
            {
                var playerName = parameters.GetParameter(2);
                toTeleport = World.GetClient(playerName);
            }

            if (toTeleport != null)
            {
                toTeleport.Teleport(mapid, cellid);
                client.ConsoleMessage("Teleportation effectuer !");
                toTeleport.Character.Save();
            }
            else
            {
                client.ConsoleMessage("Impossible de trouver le joueur", Enums.ConsoleColorEnum.RED);
            }
        }
    }
}
