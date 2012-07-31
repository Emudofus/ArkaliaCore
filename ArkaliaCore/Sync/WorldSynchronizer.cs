using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SilverSock;

namespace ArkaliaCore.Realm.Sync
{
    public class WorldSynchronizer
    {
        public Database.Models.GameServerModel Server { get; set; }
        public System.Timers.Timer SyncTimer { get; set; }
        public SilverSocket SyncSocket { get; set; }
        public object Locker = new object();

        public WorldSynchronizer(Database.Models.GameServerModel server)
        {
            this.Server = server;
        }

        public void StartSync()
        {
            this.SyncSocket = new SilverSocket();
            this.SyncSocket.OnConnected += new SilverEvents.Connected(SyncSocket_OnConnected);
            this.SyncSocket.OnDataArrivalEvent += new SilverEvents.DataArrival(SyncSocket_OnDataArrivalEvent);
            this.SyncSocket.OnFailedToConnect += new SilverEvents.FailedToConnect(SyncSocket_OnFailedToConnect);
            this.SyncSocket.OnSocketClosedEvent += new SilverEvents.SocketClosed(SyncSocket_OnSocketClosedEvent);
            this.SyncTimer = new System.Timers.Timer(5000);
            this.SyncTimer.Enabled = true;
            this.SyncTimer.Elapsed += new System.Timers.ElapsedEventHandler(RetryConnect);
            this.SyncTimer.Start();
        }

        private void SyncSocket_OnSocketClosedEvent()
        {
            Utilities.Logger.Error("@Connection lost@ with the @GameServer '" + this.Server.ID + "'@");
            this.SyncTimer.Enabled = true;
            this.SyncTimer.Start();
            this.Server.State = Enums.ServerStateEnum.Offline;
            Managers.MultiServerManager.RefreshState();
        }

        private void SyncSocket_OnFailedToConnect(Exception ex)
        {
            Utilities.Logger.Debug("@Failed@ to connect to @GameServer '" + this.Server.ID + "'@");
        }

        private void SyncSocket_OnDataArrivalEvent(byte[] data)
        {
            lock (Locker)
            {
                try
                {
                    var packet = new Interop.Crystal.CrystalPacket(data);
                    Utilities.Logger.Debug("Received packet @'" + packet.ID.ToString() + "'@ from server @'" + Server.ID + "'@");
                    switch (packet.ID)
                    {
                        case Interop.Crystal.PacketHeaderEnum.HelloKeyMessage:
                            SynchronizerHandler.HandleHelloKey(this);
                            break;

                        case Interop.Crystal.PacketHeaderEnum.PlayerConnectedMessage:
                            SynchronizerHandler.HandleConnectionPlayer(this, packet);
                            break;

                        case Interop.Crystal.PacketHeaderEnum.PlayerDisconnectedMessage:
                            SynchronizerHandler.HandleDisconnectionPlayer(this, packet);
                            break;

                        case Interop.Crystal.PacketHeaderEnum.ClientShopPointUpdateMessage:
                            SynchronizerHandler.HandleShopPointsUpdate(this, packet);
                            break;

                        case Interop.Crystal.PacketHeaderEnum.PlayerCreatedCharacterMessage:
                            SynchronizerHandler.HandleCharacterCreation(this, packet);
                            break;

                        case Interop.Crystal.PacketHeaderEnum.PlayerDeletedCharacterMessage:
                            SynchronizerHandler.HandleCharacterDeletion(this, packet);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't read packet from server : " + e.ToString());
                }          
            }
        }

        private void SyncSocket_OnConnected()
        {
            this.SyncTimer.Stop();
            this.SyncTimer.Enabled = false;
            this.SyncTimer.Close();
            this.Server.State = Enums.ServerStateEnum.Online;

            Utilities.Logger.Infos("@Synchronized@ with the @GameServer '" + this.Server.ID + "'@ (" + this.Server.Adress + ":" + this.Server.CommunicationPort + ")");
            Managers.MultiServerManager.RefreshState();
        }

        private void RetryConnect(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            this.SyncSocket.ConnectTo(this.Server.Adress, this.Server.CommunicationPort);
        }

        public void Send(Interop.Crystal.CrystalPacket packet)
        {
            try
            {
                if (this.Server.State != Enums.ServerStateEnum.Offline)
                {
                    Utilities.Logger.Debug("Send packet @'" + packet.ID.ToString() + "'@ to server @'" + Server.ID + "'@");
                    this.SyncSocket.Send(packet.GetBytes);
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't send packet to world server : " + e.ToString());
            }
        }
    }
}
