using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Operator
{
    public class SayCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "say";
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
                return "Envois un message au joueurs connecter";
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
            foreach (var player in World.GetLogged())
            {
                player.SystemMessage("<b>[" + client.Account.Pseudo + "]</b> : " + parameters.GetFullPameters, Utilities.Settings.GetSettings.GetStringElement("Colors", "SAY"));
            }
            client.ConsoleMessage("Message envoyer avec succes !");
        }
    }
}
