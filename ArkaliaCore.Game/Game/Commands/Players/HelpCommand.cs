using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Players
{
    public class HelpCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "help";
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
                return "Affiche l'aide";
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
            var infos = new StringBuilder();
            infos.Append("<b>===== Aide Arkalia =====</b><br />");
            foreach (var command in CommandManager.Commands.OrderBy(x => x.AccessLevel))
            {
                if (command.AccessLevel == 0)
                {
                    infos.Append("<b>." + command.Prefix + "</b> : " + command.Description + "<br />");
                }
                else if (command.AccessLevel > 0 && client.Account.AdminLevel >= command.AccessLevel)
                {
                    infos.Append("[<b>" + command.Prefix + "</b>] : " + command.Description + "<br />");
                }
            }
            client.SystemMessage(infos.ToString());
        }
    }
}
