using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Tables
{
    public static class NpcDialogTable
    {
        public static Dictionary<int, Models.NpcDialogModel> Cache = new Dictionary<int, Models.NpcDialogModel>();

        public static void Load()
        {
            Utilities.Logger.Infos("Loading @npcs dialogs@ ...");
            Utilities.ConsoleStyle.EnableLoadingSymbol();

            Cache.Clear();
            var reader = DatabaseManager.Provider.ExecuteReader("SELECT * FROM npc_dialog");
            while (reader.Read())
            {
                var dialog = new Models.NpcDialogModel()
                {
                    ID = reader.GetInt32("ID"),
                    ResponsesString = reader.GetString("Responses"),
                    Args = reader.GetString("Args"),
                };
                var npc = Game.Npcs.NpcManager.GetNpcForDialogID(dialog.ID);
                if (npc != null)
                {
                    npc.Dialog = dialog;
                }
                Cache.Add(dialog.ID, dialog);
            }
            reader.Close();

            Utilities.ConsoleStyle.DisabledLoadingSymbol();
            Utilities.Logger.Infos("Loaded @'" + Cache.Count + "'@ npcs dialogs");
        }
    }
}
