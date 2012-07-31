using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class CommandHandler
    {
        public static void HandleConsoleCommand(Network.Game.GameClient client, string packet)
        {
            if (client.Account.AdminLevel > 0)
            {
                var data = packet.Substring(2).Split(' ');
                var parameters = new Commands.CommandParameters(data);
                var command = Commands.CommandManager.GetCommand(parameters.Prefix.ToLower());
                if (command != null)
                {
                    command.PreExecute(client, parameters);
                }
                else
                {
                    client.ConsoleMessage("Commande invalide !", Enums.ConsoleColorEnum.RED);
                }
            }
            else
            {
                client.ConsoleMessage("Votre compte n'est pas autoriser a executer ce type de commande", Enums.ConsoleColorEnum.RED);
            }
        }
    }
}
