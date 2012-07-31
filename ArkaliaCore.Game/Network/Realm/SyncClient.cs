using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Interop.Crystal;

using SilverSock;

namespace ArkaliaCore.Game.Network.Realm
{
    public class SyncClient
    {
        public SilverSocket Socket { get; set; }

        public bool IsSecured { get; set; }
        public string Name { get; set; }

        public SyncClient(SilverSocket socket)
        {
            this.IsSecured = false;
            this.Socket = socket;
            this.Socket.OnDataArrivalEvent += new SilverEvents.DataArrival(Socket_OnDataArrivalEvent);
            this.Socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(Socket_OnSocketClosedEvent);
            this.Send(new Interop.Crystal.Packets.HelloKeyMessage());
        }

        private void Socket_OnSocketClosedEvent()
        {
            if (IsSecured)
            {
                lock (SyncServer.SecuredRealm)
                {
                    if (SyncServer.SecuredRealm.Contains(this))
                    {
                        SyncServer.SecuredRealm.Remove(this);
                    }
                }
                Utilities.Logger.Error("Secured realm @'" + this.Name + "'@ was @unsynchronized@");
            }
        }

        private void Socket_OnDataArrivalEvent(byte[] data)
        {
            try
            {
                var packet = new Interop.Crystal.CrystalPacket(data);
                if (!IsSecured)
                {
                    if (packet.ID == Interop.Crystal.PacketHeaderEnum.SecureKeyMessage)
                    {
                        this.HandleSecureKey(packet);
                    }
                    else
                    {
                        Utilities.Logger.Error("Server want to @send not allowed packet@, please @secure the link@");
                    }
                }
                else//Need to secured with secure key
                {
                    switch (packet.ID)
                    {
                        case PacketHeaderEnum.PlayerCommingMessage:
                            HandlePlayerComming(packet);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't @read packet@ from server : " + e.ToString());
            }
        }

        public void Send(Interop.Crystal.CrystalPacket packet)
        {
            try
            {
                Utilities.Logger.Debug("Send packet @'" + packet.ID.ToString() + "'@ to server");
                this.Socket.Send(packet.GetBytes);
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't send packet to server : " + e.ToString());
            }
        }

        private void HandleSecureKey(CrystalPacket packet)
        {
            string key = packet.Reader.ReadString();
            string name = packet.Reader.ReadString();
            if (key == Utilities.Settings.GetSettings.GetStringElement("Sync", "SecureKey"))
            {
                this.Name = name;

                lock(SyncServer.SecuredRealm)
                    SyncServer.SecuredRealm.Add(this);

                this.IsSecured = true;
                Utilities.Logger.Infos("Realm @'" + this.Name + "'@ synchronized @successfully@");
            }
            else
            {
                Utilities.Logger.Error("Secure @key is wrong@ from the realm @'" + name + "'@");
            }
        }

        private void HandlePlayerComming(CrystalPacket packet)
        {
            string ticket = packet.Reader.ReadString();
            var account = new ArkaliaCore.Game.Game.Models.AccountModel()
            {
                ID = packet.Reader.ReadInt32(),
                Username = packet.Reader.ReadString(),
                Password = packet.Reader.ReadString(),
                Pseudo = packet.Reader.ReadString(),
                Question = packet.Reader.ReadString(),
                Answer = packet.Reader.ReadString(),
                AdminLevel = packet.Reader.ReadInt32(),
                Points = packet.Reader.ReadInt32(),
                Vip = packet.Reader.ReadInt32(),
            };

            ArkaliaCore.Game.Game.Controllers.TicketController.RegisterTicket
                (new ArkaliaCore.Game.Game.Models.AccountTicket()
                { Ticket = ticket, Account = account, ExpireTime = 0 });
        }
    }
}
