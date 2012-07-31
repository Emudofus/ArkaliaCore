using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Chat.Channels
{
    public class DefaultChannel : Channel
    {
        public DefaultChannel()
            : base() { }

        public override string Name
        {
            get
            {
                return "Default";
            }
        }

        public override string ChannelPrefix
        {
            get
            {
                 return "*";
            }
        }

        public override bool ChannelOfficial
        {
            get
            {
                return true;
            }
        }

        public override long ChannelDelayTime
        {
            get
            {
                return 200;
            }
        }

        public override int ChannelAccountLevelAccess
        {
            get
            {
                return 0;
            }
        }

        public override int ChannelLevelAccess
        {
            get
            {
                return 0;
            }
        }

        public override void OnInitialize()
        {
            base.Locked = false;
        }

        public override void AppendMessage(Network.Game.GameClient client, string message)
        {
            if (message.StartsWith(".") && !message.StartsWith(".."))//Is a command
            {
                var parameters = new Commands.CommandParameters(message.Substring(1).Split(' '));
                var command = Commands.CommandManager.GetCommand(parameters.Prefix.ToLower());
                if (command != null)
                {
                    command.PreExecute(client, parameters);
                }
                else
                {
                    client.ErrorMessage("Commande invalide, taper <b>.help</b> pour la liste !");
                }
            }
            else//Is a simple message
            {
                client.Character.Map.Send("cMK|" + client.Character.ID + "|" + client.Character.Nickname + "|" + message);
            }
        }
    }
}
