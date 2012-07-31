using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Operator
{
    public class MapInfosCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "mapinfos";
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
                return "Affiche les informations de la carte";
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
            if (client.Character.Map != null)
            {
                client.ConsoleMessage("==== Informations of the map " + client.Character.Map + " ====");
                client.ConsoleMessage("Players count : " + client.Character.Map.Players.Count);
                client.ConsoleMessage("Maps npcs count : " + client.Character.Map.Npcs.Count);
                client.ConsoleMessage("Triggers count : " + client.Character.Map.Triggers.Count);
            }
        }
    }
}
