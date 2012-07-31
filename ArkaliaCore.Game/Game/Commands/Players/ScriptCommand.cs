using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Commands.Players
{
    public class ScriptCommand : Command
    {
        private Modules.Scripting.LuaScript _script { get; set; }
        private string _suffix { get; set; }
        private bool _needLoaded = false;
        private string _description { get; set; }

        public ScriptCommand(Modules.Scripting.LuaScript script)
        {
            this._script = script;
        }

        public override string Prefix
        {
            get
            {
                return this._suffix;
            }
            set
            {
                this._suffix = value;
            }
        }

        public override int AccessLevel
        {
            get
            {
                return 0;
            }
        }

        public override string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        public override bool NeedLoaded
        {
            get
            {
                return this._needLoaded;
            }
            set
            {
                this._needLoaded = value;
            }
        }

        public override void Execute(Network.Game.GameClient client, CommandParameters parameters)
        {
            this._script.Do(new KeyValuePair<string, object>("call_type", "command"), 
                                        new KeyValuePair<string, object>("client", client));
        }
    }
}
