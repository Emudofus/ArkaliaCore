using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class NpcHandler
    {
        public static void HandleRequestDialog(Network.Game.GameClient client, string packet)
        {
            var id = int.Parse(packet.Substring(2));
            var npc = client.Character.Map.GetNpc(id);
            if (npc != null)
            {
                if (npc.Positions.Template.Dialog != null)
                {
                    //client.Send("DCK" + npc.Positions.Template.Artwork);
                    OpenDialog(client, npc.Positions.Template.Dialog);
                }
            }
        }

        public static void HandleDialogLeave(Network.Game.GameClient client, string packet)
        {
            CloseDialog(client);
        }

        public static void HandleDialogResponse(Network.Game.GameClient client, string packet)
        {
            var data = packet.Substring(2).Split('|');
            var questionID = int.Parse(data[0]);
            var responseID = int.Parse(data[1]);

            Modules.Scripting.ScriptKernel.HandleNpcResponseScript(client, responseID);
        }

        public static void OpenDialog(Network.Game.GameClient client, Database.Models.NpcDialogModel dialog)
        {
            client.Send("DCK0");
            client.Send("DQ" + dialog.ID + "|" + dialog.ResponsesString.Replace(",", ";"));
        }

        public static void OpenDialog(Network.Game.GameClient client, int id)
        {
            var dialog = Game.Npcs.NpcManager.GetDialog(id);
            if (dialog != null)
            {
                OpenDialog(client, dialog);
            }
        }

        public static void CloseDialog(Network.Game.GameClient client)
        {
            client.Send("DV");
        }
    }
}
