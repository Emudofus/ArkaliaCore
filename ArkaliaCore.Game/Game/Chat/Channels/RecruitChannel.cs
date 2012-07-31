using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Chat.Channels
{
    public class RecruitChannel : Channel
    {
        public RecruitChannel()
            : base() { }

        public override string Name
        {
            get
            {
                return "Recruit";
            }
        }

        public override string ChannelPrefix
        {
            get
            {
                 return "?";
            }
        }

        public override bool ChannelOfficial
        {
            get
            {
                return true;
            }
        }

        public override long ChannelDelayTime
        {
            get
            {
                return 20000;
            }
        }

        public override int ChannelAccountLevelAccess
        {
            get
            {
                return 0;
            }
        }

        public override int ChannelLevelAccess
        {
            get
            {
                return 10;
            }
        }

        public override void OnInitialize()
        {
            base.Locked = false;
        }

        public override void AppendMessage(Network.Game.GameClient client, string message)
        {
            World.Send("cMK"+ this.ChannelPrefix + "|" + client.Character.ID + "|" + client.Character.Nickname + "|" + message);
        }
    }
}
