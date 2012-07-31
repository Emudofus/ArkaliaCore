using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Players
{
    public class InfosCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "infos";
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
                return "Affiche les informations serveur";
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
            infos.Append("Core <b>Arkalia</b> v" + Definitions.CoreVersion + " par <b>NightWolf</b><br />");
            infos.Append("Joueurs en ligne : <b>" + Network.Game.GameServer.Clients.Count + "</b><br />");
            infos.Append("Uptime : <b>" + Utilities.Basic.GetUptime() + "</b>");
            client.SystemMessage(infos.ToString());
        }
    }
}
