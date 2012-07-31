using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class EmoteHandler
    {
        public static void HandleDoEmote(Network.Game.GameClient client, string packet)
        {
            var emoteID = (Enums.EmoteTypeEnum)int.Parse(packet.Substring(2));

            //Special effects
            switch (emoteID)
            {
                case Enums.EmoteTypeEnum.SIT:
                    client.StartAutoRegen();
                    break;

                case Enums.EmoteTypeEnum.CHAMPION:
                    foreach (var player in client.Character.Map.Players)
                    {
                        if (player.Character.ID != client.Character.ID)
                        {
                            DoEmote(player, Enums.EmoteTypeEnum.APPL);
                        }
                    }
                    break;
            }

            DoEmote(client, emoteID);
        }

        public static void DoEmote(Network.Game.GameClient client, Enums.EmoteTypeEnum emote)
        {
            client.Character.Map.Send("eUK" + client.Character.ID + "|" + (int)emote);
        }

        public static void HandleDoSmiley(Network.Game.GameClient client, string packet)
        {
            var smileyID = int.Parse(packet.Substring(2));
            if (client.Character.Map != null)
            {
                client.Character.Map.Send("cS" + client.Character.ID + "|" + smileyID);
            }
        }
    }
}
