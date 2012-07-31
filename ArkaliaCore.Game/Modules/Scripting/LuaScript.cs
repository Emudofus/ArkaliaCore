using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

using LuaInterface;

namespace ArkaliaCore.Game.Modules.Scripting
{
    public class LuaScript
    {
        public string Path { get; set; }
        public Lua Interpreter = new Lua();
        public bool _intialized = false;

        public Dictionary<string, object> StaticParameters = new Dictionary<string, object>();
        public Dictionary<string, object> DynamicParameters = new Dictionary<string, object>();

        public LuaScript(string path)
        {
            this.Path = path;
            this.Interpreter.RegisterFunction("get_param", this, this.GetType().GetMethod("GetParameter"));
            this.Interpreter.RegisterFunction("is_init", this, this.GetType().GetMethod("IsInitialized"));
            this.Interpreter.RegisterFunction("log", this, this.GetType().GetMethod("Log"));
            this.Interpreter.RegisterFunction("tickcount", this, this.GetType().GetMethod("GetTickcount"));
            this.Interpreter.RegisterFunction("scache", this, this.GetType().GetMethod("SetCacheValue"));
            this.Interpreter.RegisterFunction("gcache", this, this.GetType().GetMethod("GetCacheValue"));
            this.Interpreter.RegisterFunction("params_infos", this, this.GetType().GetMethod("GetParametersInfos"));

            this.Interpreter.RegisterFunction("register_command", this, this.GetType().GetMethod("RegisterCommand"));
            this.Interpreter.RegisterFunction("register_responses", this, this.GetType().GetMethod("RegisterResponse"));
            this.Interpreter.RegisterFunction("register_events", this, this.GetType().GetMethod("RegisterEvents"));

            //this.Interpreter.RegisterFunction("client_teleport", this, this.GetType().GetMethod("ClientTeleport"));
            //this.Interpreter.RegisterFunction("client_message", this, this.GetType().GetMethod("ClientMessage"));
            this.Interpreter.RegisterFunction("client_open_dialog", this, this.GetType().GetMethod("ClientOpenDialog"));
            this.Interpreter.RegisterFunction("client_close_dialog", this, this.GetType().GetMethod("ClientCloseDialog"));
            //this.Interpreter.RegisterFunction("client_save_position", this, this.GetType().GetMethod("ClientSavePosition"));

            this.Interpreter.RegisterFunction("get_npc_on_map", this, this.GetType().GetMethod("GetNpcOnMap"));
        }

        public void Do(params KeyValuePair<string, object>[] values)
        {
            lock (StaticParameters)
            {
                try
                {
                    foreach (var p in values)
                    {
                        StaticParameters.Add(p.Key, p.Value);
                    }

                    this.Interpreter.DoFile(this.Path);
                    this.StaticParameters.Clear();
                    this._intialized = true;
                }
                catch (Exception e)
                {
                    this.StaticParameters.Clear();
                    Utilities.Logger.Error("Can't execute script @'" + this.Path + "'@ : " + e.ToString());
                }
            }
        }

        public bool IsInitialized()
        {
            return this._intialized;
        }

        public object GetParameter(string key)
        {
            if (this.StaticParameters.ContainsKey(key))
            {
                return this.StaticParameters[key];
            }
            else
            {
                return null;
            }
        }

        public string GetParametersInfos()
        {
            return string.Join(",", this.StaticParameters);
        }

        #region API Methods

        public static void Log(string message)
        {
            Utilities.Logger.Script(message);
        }

        public static void RegisterCommand(string prefix, string description, string luaScript)
        {
            var script = new LuaScript(luaScript);
            script.Do();
            Game.Commands.CommandManager.Add(prefix, new Game.Commands.Players.ScriptCommand(script) { Prefix = prefix,
                NeedLoaded = true, Description = description });
        }

        public static void RegisterResponse(string luaScript)
        {
            var script = new LuaScript(luaScript);
            script.Do();
            ScriptKernel.ResponsesScripts.Add(script);
        }

        public static void RegisterEvents(string luaScript)
        {
            var script = new LuaScript(luaScript);
            script.Do();
            ScriptKernel.EventsScripts.Add(script);
        }

        public void SetCacheValue(string key, object obj)
        {
            if (DynamicParameters.ContainsKey(key))
            {
                DynamicParameters[key] = obj;
            }
            else
            {
                DynamicParameters.Add(key, obj);
            }
        }

        public object GetCacheValue(string key)
        {
            if (DynamicParameters.ContainsKey(key))
            {
                return DynamicParameters[key];
            }
            else
            {
                return null;
            }
        }

        public long GetTickcount()
        {
            return Environment.TickCount;
        }

        public Game.Npcs.NpcInstance GetNpcOnMap(int mapid, int templateID)
        {
            var map = Game.Controllers.MapController.GetMap(mapid);

            if (map != null)
            {
                var npc = map.GetNpcByTemplate(templateID);
                return npc;
            }
            else
            {
                return null;
            }
        }

        #region Clients API

        //public static void ClientTeleport(Network.Game.GameClient client, int mapid, int cellid)
        //{
        //    client.Teleport(mapid, cellid);
        //}

        //public static void ClientMessage(Network.Game.GameClient client, string message, string color = "system")
        //{
        //    client.SystemMessage(message, color);
        //}

        public static void ClientOpenDialog(Network.Game.GameClient client, int id)
        {
            Game.Handlers.NpcHandler.OpenDialog(client, id);
        }

        public static void ClientCloseDialog(Network.Game.GameClient client)
        {
            Game.Handlers.NpcHandler.CloseDialog(client);
        }

        //public static void ClientSavePosition(Network.Game.GameClient client)
        //{
        //    client.SavePosition();
        //}

        #endregion

        #endregion
    }
}
