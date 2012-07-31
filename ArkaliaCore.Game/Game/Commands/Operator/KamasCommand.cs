using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Operator
{
    public class KamasCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "kamas";
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
                return "Ajoute des kamas";
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
            var quantity = parameters.GetIntParameter(0);
            var to = client;

            if (parameters.Lenght > 1)
            {
                var name = parameters.GetParameter(1);
                to = World.GetClient(name);
            }

            if (to != null)
            {
                to.Character.Kamas += quantity;
                to.SendStats();
                client.ConsoleMessage("Kamas ajouter");
                client.Character.Save();
            }
            else
            {
                client.ConsoleMessage("Impossible de trouver le joueur", Enums.ConsoleColorEnum.RED);
            }
        }
    }
}
