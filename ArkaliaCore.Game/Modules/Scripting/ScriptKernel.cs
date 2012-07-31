using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ArkaliaCore.Game.Modules.Scripting
{
    public static class ScriptKernel
    {
        public static List<LuaScript> Scripts = new List<LuaScript>();
        public static List<LuaScript> ResponsesScripts = new List<LuaScript>();
        public static List<LuaScript> EventsScripts = new List<LuaScript>();

        public static void Load()
        {
            Scripts.Clear();
            ResponsesScripts.Clear();
            EventsScripts.Clear();
            foreach (var scriptPath in Directory.GetFiles("Scripts"))
            {
                try
                {
                    var script = new LuaScript(scriptPath);
                    script.Do(new KeyValuePair<string, object>("core_version", Definitions.CoreVersion));
                    Scripts.Add(script);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't load script @'" + scriptPath + "'@ : " + e.ToString());
                }
            }
            Utilities.Logger.Infos("Loaded @'" + Scripts.Count + "'@ scripts");
        }

        public static void HandleNpcResponseScript(Network.Game.GameClient client, int responseID, params KeyValuePair<string, object>[] values)
        {
            var listValues = values.ToList();
            listValues.Add(new KeyValuePair<string, object>("client", client));
            listValues.Add(new KeyValuePair<string, object>("response_id", responseID));
            foreach (var script in ResponsesScripts)
            {
                script.Do(listValues.ToArray());
            }
        }

        public static void HandlePlayerEvents(Network.Game.GameClient client, string eventType, params KeyValuePair<string, object>[] values)
        {
            var listValues = values.ToList();
            listValues.Add(new KeyValuePair<string, object>("client", client));
            listValues.Add(new KeyValuePair<string, object>("t_event", eventType));
            foreach (var script in EventsScripts)
            {
                try
                {
                    script.Do(listValues.ToArray());
                }
                catch (Exception e)
                {
                    script.StaticParameters.Clear();
                }
            }
        }
    }
}
