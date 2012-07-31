using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Operator
{
    public class RegenCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "regen";
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
                return "Remet votre vie au maximum";
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
            client.Character.CurrentLife = client.Character.MaxLife;
            client.SendStats();
            client.ConsoleMessage("Votre vie est desormais a son maximum !", Enums.ConsoleColorEnum.GREEN);
        }
    }
}
