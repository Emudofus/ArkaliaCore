using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Commands
{
    public abstract class Command
    {
        public virtual string Prefix { get; set; }
        public virtual int AccessLevel { get; set; }
        public virtual string Description { get; set; }

        public virtual bool NeedLoaded { get; set; }

        public bool Locked = false;

        public void PreExecute(Network.Game.GameClient client, CommandParameters parameters)
        {
            if (!Locked)
            {
                if (client.Account.AdminLevel >= AccessLevel)
                {
                    Execute(client, parameters);
                }
            }
        }

        public virtual void Execute(Network.Game.GameClient client, CommandParameters parameters)
        {
            
        }
    }
}
