using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class FriendHandler
    {
        public static void HandleRequestFriends(Network.Game.GameClient client, string packet)
        {
            client.SendFriends();
        }

        public static void HandleAddFriend(Network.Game.GameClient client, string packet)
        {
            if (packet.Length > 3)
            {
                var data = packet.Substring(2);
                var type = data[0];
                switch (type)
                {
                    case '%'://Account
                        var name = data.Substring(1).ToLower();

                        //He have no friends :(
                        if (name == client.Account.Pseudo.ToLower() || name == client.Character.Nickname.ToLower())
                        {
                            client.ErrorMessage("Vous ne pouvez vous ajouter en ami, chercher plutot de <b>\"vrai\"</b> ami(s) ! :(");
                            return;
                        }

                        var accountFinded = World.GetClientByPseudo(name);
                        if (accountFinded != null)
                        {
                            Utilities.Logger.Debug("Try to add friend by account [" + name + "]");
                            if (!client.Account.Infos.HaveFriend(accountFinded.Account.ID))
                            {
                                client.Account.Infos.Friends.Add(new Friends.Friend(accountFinded.Account.Infos.AccountId));
                                client.SendFriends();
                                client.Account.Infos.Save();
                            }
                            else
                            {
                                //TODO: Have already friend
                            }
                        }
                        else//Try find by character name
                        {
                            addByCharacter(client, name);
                        }
                        break;

                    default://Character name
                        addByCharacter(client, data);
                        break;
                }
            }
        }

        public static void HandleDeleteFriend(Network.Game.GameClient client, string packet)
        {
            if (packet.Length > 3)
            {
                var data = packet.Substring(2);
                var type = data[0];
                switch (type)
                {
                    case '*':
                        var name = data.Substring(1);
                        var friend = client.Account.Infos.GetFriendByNickname(name);
                        if (friend != null)
                        {
                            client.Account.Infos.Friends.Remove(friend);
                            client.SendFriends();
                            client.Account.Infos.Save();
                        }
                        else
                        {
                            client.ErrorMessage("Vous ne posseder pas cette personne en ami !");
                        }
                        break;
                }
            }
        }

        private static void addByCharacter(Network.Game.GameClient client, string name)
        {
            Utilities.Logger.Debug("Try to add friend by character [" + name + "]");
            var player = World.GetClient(name);
            if (player != null)
            {
                if (!client.Account.Infos.HaveFriend(player.Account.ID))
                {
                    client.Account.Infos.Friends.Add(new Friends.Friend(player.Account.Infos.AccountId));
                    client.SendFriends();
                    client.Account.Infos.Save();
                }
                else
                {
                    //TODO: Have already friend
                }
            }
            else
            {
                client.Send("cMEf" + name);
            }
        }
    }
}
