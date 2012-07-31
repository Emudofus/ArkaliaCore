using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class GameMapHandler
    {
        public static void HandleGameCreate(Network.Game.GameClient client, string packet)
        {
            int typeCreate = int.Parse(packet.Substring(2));
            switch (typeCreate)
            {
                case 1://Roleplay type
                    if (client.FirstConnection)
                    {
                        client.Send("GCK|1|" + client.Character.Nickname);
                        client.Send("cC+*#$pi:?%");
                        client.Send("AR6bk");

                        if (Utilities.Settings.GetSettings.GetStringElement("Global", "MOTD") != "")
                        {
                            client.SystemMessage(Utilities.Settings.GetSettings.GetStringElement("Global", "MOTD"),
                                                        Utilities.Settings.GetSettings.GetStringElement("Colors", "MOTD"));
                        }
                        client.Teleport(client.Character.MapID, client.Character.CellID);

                        client.Send("SLo+");
                        client.SendStats();
                        client.SendEmotes();
                        client.SendJobs();
                        client.RefreshPods();
                        client.WarnConnection();
                        client.FirstConnection = false;
                    }
                    else
                    {

                    }
                    break;
            }
        }

        public static void HandleGameInformationsRequest(Network.Game.GameClient client, string packet)
        {
            if (client.Character.Map != null)
            {
                client.Character.Map.ShowPlayers(client);
                client.Character.Map.ShowNpcs(client);
            }
            client.Send("GDK");
        }
    }
}
