﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Chat.Channels
{
    public class AdminSubChannel : Channel
    {
        public AdminSubChannel()
            : base() { }

        public override string Name
        {
            get
            {
                return "AdminSub";
            }
        }

        public override string ChannelPrefix
        {
            get
            {
                return "Â¤";
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
                return 0;
            }
        }

        public override int ChannelAccountLevelAccess
        {
            get
            {
                return 1;
            }
        }

        public override int ChannelLevelAccess
        {
            get
            {
                return 1;
            }
        }

        public override void OnInitialize()
        {
            base.Locked = true;
        }

        public override void AppendMessage(Network.Game.GameClient client, string message)
        {
            World.SendAdmin("cMK"+ this.ChannelPrefix + "|" + client.Character.ID + "|" + client.Character.Nickname + "|" + message);
        }
    }
}
