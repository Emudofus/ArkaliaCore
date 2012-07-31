using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Chat.Channels
{
    public abstract class Channel
    {
        public virtual string Name { get; set; }
        public virtual string ChannelPrefix { get; set; }
        public virtual int ChannelAccountLevelAccess { get; set; }
        public virtual int ChannelLevelAccess { get; set; }
        public virtual long ChannelDelayTime { get; set; }
        public virtual bool ChannelOfficial { get; set; }

        public bool Locked = false;

        protected Utilities.FileBuffer _channelBuffer { get; set; }

        public Channel()
        {
            this._channelBuffer = new Utilities.FileBuffer("Logs/Channels/channel_" + Name.ToLower() + "_at_" + Utilities.Logger.GetFormattedDate + ".txt");
            this.OnInitialize();
        }

        public virtual void OnInitialize()
        {
            throw new NotImplementedException();
        }

        public virtual void PreExecuteMessage(Network.Game.GameClient client, string message)
        {
            if (!Locked)
            {
                if (client.Account.AdminLevel >= ChannelAccountLevelAccess)
                {
                    if (client.Character.Level >= ChannelLevelAccess)
                    {
                        AppendMessage(client, message);
                        this._channelBuffer.WriteLine("From " + client.Character.Nickname + " : " + message);
                    }
                    else
                    {
                        client.SystemMessage("Il est necessaire d'etre niveau <b>" + ChannelLevelAccess + "</b> pour parler dans ce canal");
                    }
                }
                else
                {
                    client.SystemMessage("Votre compte n'est pas autoriser a parler dans ce canal !");
                }
            }
            else
            {
                client.SystemMessage("Ce canal est <b>verrouiler</b> !");
            }
        }

        public virtual void AppendMessage(Network.Game.GameClient client, string message)
        {
            throw new NotImplementedException();
        }
    }
}
