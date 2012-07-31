using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

using SilverSock;

namespace ArkaliaCore.Game.Network.Game
{
    public class GameClient
    {
        public SilverSocket Socket { get; set; }

        public ArkaliaCore.Game.Game.Models.AccountModel Account { get; set; }
        public List<Database.Models.CharacterModel> Characters { get; set; }
        public Database.Models.CharacterModel Character { get; set; }

        public bool FirstConnection = true;

        /* Game Contents */

        public PathEngine CurrentMove { get; set; }
        public Enums.GameObjectTypeEnum CurrentObjectUsed = Enums.GameObjectTypeEnum.NONE;
        public long RegenBaseTime { get; set; }

        /* ====End==== */

        public GameClient(SilverSocket socket)
        {
            this.Socket = socket;
            this.Socket.OnDataArrivalEvent += new SilverEvents.DataArrival(Socket_OnDataArrivalEvent);
            this.Socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(Socket_OnSocketClosedEvent);
            this.Send("HG");
        }

        public void Send(string packet)
        {
            try
            {
                byte[] data = Encoding.Default.GetBytes(packet + "\x00");
                this.Socket.Send(data);
                Utilities.Logger.Debug("Sended @packet to client@ : " + packet);
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't @send packet@ : " + packet);
            }
        }

        public void Kick()
        {
            try
            {
                this.Socket.CloseSocket();
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't @disconnect client@ : " + e.ToString());
            }
        }

        #region Events

        public void Socket_OnDataArrivalEvent(byte[] data)
        {
            string noParsed = Encoding.Default.GetString(data);
            foreach (string packet in noParsed.Replace("\x0a", "").Split('\x00'))
            {
                try
                {
                    if (packet == "")
                        continue;
                    Utilities.Logger.Debug("Received @packet from client@ : " + packet);
                    this.HandlePacket(packet);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't @parse@ packet : " + e.ToString());
                }
            }
        }

        private void Socket_OnSocketClosedEvent()
        {
            try
            {
                if (this.Account != null)
                {
                    if (this.Character != null)
                    {
                        this.Character.Save();
                        if (this.Character.Map != null)//Remove player from the map
                        {
                            this.Character.Map.RemovePlayer(this);
                        }
                    }
                    Realm.SyncServer.SendToSecuredRealm(new Interop.Crystal.Packets.PlayerDisconnectedMessage(this.Account.Username));
                    Utilities.Logger.Infos("Player @'" + this.Account.Username + "'@ disconnected");
                }
                else
                {
                    Utilities.Logger.Infos("Gameclient @disconnected@ !");
                }
                lock (GameServer.Clients)
                {
                    if (GameServer.Clients.Contains(this))//Already true but i want to check this :)
                    {
                        GameServer.Clients.Remove(this);
                    }
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't @disconnect@ client : " + e.ToString());
            }
        }

        #endregion
    }
}
