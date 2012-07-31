using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SilverSock;

namespace ArkaliaCore.Realm.Network
{
    public class RealmClient
    {
        public SilverSocket Socket { get; set; }

        public RealmState State = RealmState.VERSION;
        public string EncryptKey { get; set; }

        public Database.Models.AccountModel Account { get; set; }
        public List<Database.Models.AccountCharacterModel> Characters = new List<Database.Models.AccountCharacterModel>();

        public RealmClient(SilverSocket socket)
        {
            this.Socket = socket;
            this.Socket.OnDataArrivalEvent += new SilverEvents.DataArrival(Received);
            this.Socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(Disconnected);
            this.EncryptKey = Utilities.Basic.RandomString(32);
            this.Send("HC" + this.EncryptKey);
        }

        public void Send(string packet)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(packet + "\x00");
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

        public void Disconnected()
        {
            try
            {
                lock (RealmServer.Clients)
                    RealmServer.Clients.Remove(this);

                if (this.Account != null)
                {
                    Database.Tables.AccountTable.UpdateLogged(this.Account.Username, 0);
                }

                Utilities.Logger.Infos("Client disconnected !");
            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't disconnect player : " + e.ToString());
            }
        }

        public void Received(byte[] data)
        {
            string noParsed = Encoding.ASCII.GetString(data);
            try
            {
                if (noParsed.Length > 150)
                {
                    this.Socket.CloseSocket();
                    Utilities.Logger.Error("Client kicked beaucause: Packet flood");
                }

            }
            catch (Exception e)
            {
                Utilities.Logger.Error("Can't kick packet : " + e.ToString());
            }
            foreach (string packet in noParsed.Replace("\x0a", "").Split('\x00'))
            {
                try
                {
                    if (packet == "")
                        continue;
                    Utilities.Logger.Debug("Received @packet from client@ : " + packet);
                    this.Dispatch(packet);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't parse packet : " + e.ToString());
                }
            }
        }

        public void Dispatch(string packet)
        {
            if (packet == "Af") return;
            switch (this.State)
            {
                case RealmState.VERSION:
                    HandleVersion(packet);
                    break;

                case RealmState.ACCOUNT:
                    HandleAccount(packet);
                    break;

                case RealmState.SERVER_LIST:
                    HandleServerList(packet);
                    break;
            }
        }

        public void HandleVersion(string version)
        {
            if (version == Definitions.DofusVersionRequired)
            {
                this.State = RealmState.ACCOUNT;
            }
            else
            {
                Utilities.Logger.Error("Client has incorrect version of dofus @'" + version + "'@");
                this.Send("AlEv" + Definitions.DofusVersionRequired);
            }
        }

        public void HandleAccount(string packet)
        {
            string[] data = packet.Split('#');
            string username = data[0];
            string password = data[1].Substring(1);//Sub one, because as a const char '1'
            var account = Database.Tables.AccountTable.GetAccountFromSQL(username);
            if (account != null)
            {
                if (Utilities.Hash.CryptPass(this.EncryptKey, account.Password) == password)
                {
                    this.Account = account;
                    Database.Tables.AccountTable.UpdateLogged(this.Account.Username, 1);
                    this.Send("Ad" + this.Account.Pseudo);
                    this.Send("Ac0");
                    this.Characters = Managers.MultiServerManager.GetCharactersInformations(this.Account);
                    Managers.MultiServerManager.SendServerState(this);
                    this.Send("AlK" + (this.Account.AdminLevel > 0 ? 1 : 0));
                    this.State = RealmState.SERVER_LIST;
                    Utilities.Logger.Infos("Client was logged with the account @'" + username + "'@");
                    lock (Managers.MultiServerManager.ConnectedAccounts)
                    {
                        if (Managers.MultiServerManager.ConnectedAccounts.Contains(account.Username))
                        {
                            Managers.MultiServerManager.Send(new Interop.Crystal.Packets.KickPlayerMessage(account.Username));
                        }
                    }
                }
                else
                {
                    this.Send("AlEx");
                    Utilities.Logger.Error("Wrong password for account @'" + username + "'@");
                }
            }
            else
            {
                this.Send("AlEx");
                Utilities.Logger.Error("Can't found account @'" + username + "'@");
            }
        }

        public void HandleServerList(string packet)
        {
            if (packet.Substring(0, 2) == "Ax")
            {
                 Managers.MultiServerManager.SendCharacterCount(this);
            }
            else if (packet.Substring(0, 2) == "AX")
            {
                HandleGotoServer(packet);
            }
        }

        public void HandleGotoServer(string packet)
        {
            int serverID = int.Parse(packet.Substring(2));
            var synchronizer = Managers.MultiServerManager.GetSync(serverID);
            string ticket = Utilities.Basic.RandomString(64);
            if (synchronizer.Server.State == Enums.ServerStateEnum.Online)
            {
                synchronizer.Send(new Interop.Crystal.Packets.PlayerCommingMessage(this.Account, ticket));
                this.Send("AYK" + synchronizer.Server.Adress + ":" + synchronizer.Server.GamePort + ";" + ticket);
            }
        }
    }
}
