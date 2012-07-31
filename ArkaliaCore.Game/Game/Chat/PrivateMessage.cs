using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Chat
{
    public class PrivateMessage
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }

        public PrivateMessage(string sender, string receiver, string message)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Message = message;
        }

        public void SendMessage()
        {
            var sender = World.GetClient(this.Sender);
            var receiver = World.GetClient(this.Receiver);

            if (receiver != null)
            {
                if (sender != null)
                {
                    sender.Send("cMKT|" + sender.Character.ID + "|" + this.Receiver + "|" + this.Message);
                }
                receiver.Send("cMKF|" + sender.Character.ID + "|" + sender.Character.Nickname + "|" + this.Message);
            }
            else//Receiver not connected
            {
                if (sender != null)
                {
                    sender.Send("cMEf" + this.Receiver);
                }
            }
        }
    }
}
