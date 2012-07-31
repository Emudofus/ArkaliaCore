using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class ChatHandler
    {
        public static void HandleMessageRequest(Network.Game.GameClient client, string packet)
        {
            var data = packet.Substring(2).Split('|');
            var channelPrefix = data[0];
            var message = data[1];
            if (message.Length <= 100)
            {
                var channel = Chat.ChannelManager.GetChannel(channelPrefix);
                if (channel != null)//Basic channel
                {
                    channel.PreExecuteMessage(client, message);
                }
                else//Private message
                {
                    var nickname = channelPrefix;
                    var privateMessage = new Chat.PrivateMessage(client.Character.Nickname, nickname, message);
                    privateMessage.SendMessage();
                }
            }
            else
            {
                client.ErrorMessage("Votre message est trop long pour etre envoyer");
            }
        }
    }
}
