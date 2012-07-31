using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class AccountHandler
    {
        public static void HandleTicket(Network.Game.GameClient client, string packet)
        {
            string ticket = packet.Substring(2);
            var a_ticket = Controllers.TicketController.GetTicket(ticket);
            if (a_ticket != null)
            {
                client.Account = a_ticket.Account;
                client.Characters = Controllers.CharacterController.GetCharacterForOwner(client.Account.ID);
                Network.Realm.SyncServer.SendToSecuredRealm(new Interop.Crystal.Packets.PlayerConnectedMessage(client.Account.Username));
                SendCharactersList(client);

                if (!Accounts.AccountManager.HaveAccountInformations(client.Account.Username))
                {
                    Accounts.AccountManager.CreateAccountInformations(client.Account);
                }

                client.Account.Infos = Accounts.AccountManager.GetAccountInformations(client.Account.Username);

                Utilities.Logger.Infos("Player @'" + client.Account.Username + "'@ logged !");
            }
            else
            {
                client.Send("ATE");
                Utilities.Logger.Error("Wrong @ticket from client@, maybe a @problem in realm connection@ ?");
            }
        }

        public static void HandleCharacterDeletionRequest(Network.Game.GameClient client, string packet)
        {
            int id = int.Parse(packet.Substring(2).Replace("|", ""));
            if(client.Characters != null)
            {
                var character = client.Characters.FirstOrDefault(x => x.ID == id);
                Database.Tables.CharacterTable.Delete(character);
                foreach (var item in character.Bag.Stacks.ToArray()) { character.Bag.Remove(item, item.WItem.Quantity); }
                client.Characters.Remove(character);
                Network.Realm.SyncServer.SendToSecuredRealm(new Interop.Crystal.Packets.PlayerDeletedCharacterMessage(character.Nickname));
                SendCharactersList(client);
                Utilities.Logger.Infos("Character @'" + character.Nickname + "'@ deleted !");
            }
        }

        public static void HandleCharacterCreationRequest(Network.Game.GameClient client, string packet)
        {
            //Parse data received
            string[] data = packet.Substring(2).Split('|');
            string nickname = data[0];
            int breed = int.Parse(data[1]);
            int gender = int.Parse(data[2]);
            int color1 = int.Parse(data[3]);
            int color2 = int.Parse(data[4]);
            int color3 = int.Parse(data[5]);

            //Anti-Cheat checking
            if (breed < 0 || breed > 12)
            {
                client.Send("AAEx");
                return;
            }
            if (gender != 0 && gender != 1)
            {
                client.Send("AAEx");
                return;
            }

            if (Controllers.CharacterController.ValidNickname(nickname))
            {
                if (!Controllers.CharacterController.ExistCharacter(nickname))
                {
                    //Apply parsed data
                    var character = new Database.Models.CharacterModel()
                    {
                        ID = Controllers.CharacterController.AvailableID(),
                        Nickname = nickname,
                        Breed = breed,
                        Gender = gender,
                        Color1 = color1,
                        Color2 = color2,
                        Color3 = color3,
                        Scal = 100,

                        //Native configuration
                        Owner = client.Account.ID,
                        MapID = Utilities.Settings.GetSettings.GetIntElement("Start", "Map"),
                        CellID = Utilities.Settings.GetSettings.GetIntElement("Start", "Cell"),
                        SaveMap = Utilities.Settings.GetSettings.GetIntElement("Start", "Map"),
                        SaveCell = Utilities.Settings.GetSettings.GetIntElement("Start", "Cell"),
                        Direction = Utilities.Settings.GetSettings.GetIntElement("Start", "Direction"),
                        Level = Utilities.Settings.GetSettings.GetIntElement("Start", "Level"),
                    };

                    //Complement the character
                    character.CaractPoint = (character.Level * 5) - 5;
                    character.SpellPoint = character.Level - 1;
                    character.Experience = character.LevelFloor.Character;
                    character.Look = Controllers.CharacterController.BaseLook(character);
                    character.CurrentLife = character.MaxLife;

                    //Save the character in cache and bdd
                    Database.Tables.CharacterTable.Insert(character);
                    lock (Database.Tables.CharacterTable.Cache)
                        Database.Tables.CharacterTable.Cache.Add(character.ID, character);

                    client.Characters.Add(character);

                    //Send to realm the creation and show the character for the client
                    Network.Realm.SyncServer.SendToSecuredRealm(new Interop.Crystal.Packets.PlayerCreatedCharacterMessage(character));
                    SendCharactersList(client);
                }
                else
                {
                    client.Send("AAEa");
                }
            }
            else
            {
                client.Send("AAEa");
            }
        }

        public static void HandleSelectCharacter(Network.Game.GameClient client, string packet)
        {
            int id = int.Parse(packet.Substring(2));
            var character = client.Characters.FirstOrDefault(x => x.ID == id);
            if (character != null)
            {
                client.Character = character;
                client.Send("ASK|" + character.SelectedPattern);
            }
        }

        public static void HandleRandomNameCharacter(Network.Game.GameClient client, string packet)
        {
            var name = Utilities.Basic.GetRandomName();
            client.Send("APK" + name);
        }

        public static void SendCharactersList(Network.Game.GameClient client)
        {
            var packet = new StringBuilder("ALK316000000000|");//TODO: Subscription
            packet.Append(client.Characters.Count);
            foreach (var character in client.Characters)
            {
                packet.Append("|").Append(character.SelectionPattern);
            }
            client.Send(packet.ToString());
        }
    }
}
