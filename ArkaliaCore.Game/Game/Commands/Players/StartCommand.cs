using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Players
{
    public class StartCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "start";
            }
        }

        public override int AccessLevel
        {
            get
            {
                return 0;
            }
        }

        public override string Description
        {
            get
            {
                return "Retourne au point de depart";
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
            client.Teleport(client.Character.SaveMap, client.Character.SaveCell);
            client.SystemMessage("Teleporter au point de depart !");
        }
    }
}
