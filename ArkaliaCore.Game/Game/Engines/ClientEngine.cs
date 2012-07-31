using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Engines
{
    public static class ClientEngine
    {
        public static void SystemMessage(this Network.Game.GameClient client, string message, string color = "system")
        {
            if (color == "system")
            {
                client.Send("cs<font color=\"" + Utilities.Settings.GetSettings.GetStringElement("Colors", "SYSTEM") + "\">" + message + "</font>");
            }
            else
            {
                client.Send("cs<font color=\"" + color + "\">" + message + "</font>");
            }
        }

        public static void ErrorMessage(this Network.Game.GameClient client, string message)
        {
            client.Send("Im116;<b>[Erreur]</b>~" + message);
        }

        public static void ConsoleMessage(this Network.Game.GameClient client, string message, Enums.ConsoleColorEnum color = Enums.ConsoleColorEnum.GREEN)
        {
            client.Send("BAT" + (int)color + message);
        }

        public static void SendImPacket(this Network.Game.GameClient client, string header, string param = "")
        {
            client.Send("Im" + header + ";" + param);
        }

        public static void Teleport(this Network.Game.GameClient client, int mapid, int cellid)
        {
            if (client.Character.Map != null)
            {
                client.Character.Map.RemovePlayer(client);
            }

            client.Character.MapID = mapid;
            client.Character.CellID = cellid;

            client.Character.Map.AddPlayer(client);

            client.Character.Map.ShowMap(client);
        }

        public static void SendStats(this Network.Game.GameClient client)
        {
            client.Send("As" + client.Character.StatsPattern);
        }

        public static void Regen(this Network.Game.GameClient client, int value)
        {
            if (client.Character.MaxLife != client.Character.CurrentLife)
            {
                int regenPoints = value;
                if ((client.Character.CurrentLife + value) > client.Character.MaxLife)
                {
                    regenPoints = client.Character.MaxLife - client.Character.CurrentLife;
                }
                client.Character.CurrentLife += value;
                if (client.Character.CurrentLife > client.Character.MaxLife)
                {
                    client.Character.CurrentLife = client.Character.MaxLife;
                }
                client.SendStats();
                client.Send("Im01;" + regenPoints);
            }
        }

        public static void StartAutoRegen(this Network.Game.GameClient client, int intervall = 1000)
        {
            EndAutoRegen(client);
            client.RegenBaseTime = Environment.TickCount;
            client.Send("ILS" + intervall);
        }

        public static void EndAutoRegen(this Network.Game.GameClient client)
        {
            if (client.RegenBaseTime != 0)
            {
                try
                {
                    var regenTime = (Environment.TickCount - client.RegenBaseTime) / 1000;
                    Utilities.Logger.Debug("Regen time : " + regenTime);
                    Regen(client, (int)regenTime);
                    client.RegenBaseTime = 0;
                    client.Send("ILS" + int.MaxValue);
                    client.SendStats();
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't apply regen : " + e.ToString());
                }
            }
        }

        public static void SendEmotes(this Network.Game.GameClient client)
        {
            if (client.Character != null)
            {
                client.Send("eL" + client.Character.EmoteClient + "|0");
            }
        }

        public static void SendJobs(this Network.Game.GameClient client)
        {
            if (client.Character != null)
            {
                var jsPacket = new StringBuilder("JS");
                foreach (var job in client.Character.Jobs)
                {
                    jsPacket.Append("|").Append(job.GetAdvancedPattern);
                }
                client.Send(jsPacket.ToString());
                var jxPacket = new StringBuilder("JX");
                foreach (var job in client.Character.Jobs)
                {
                    jxPacket.Append("|").Append(job.GetBasicPattern);
                }
                client.Send(jxPacket.ToString());
            }
        }

        public static void SendFriends(this Network.Game.GameClient client)
        {
            var packet = new StringBuilder("FL");
            lock (client.Account.Infos.Friends)
            {
                foreach (var friend in client.Account.Infos.Friends)
                {
                    packet.Append("|").Append(friend.Account.Username);
                    var player = World.GetClientByAccount(friend.Account.AccountId);
                    if (player != null)
                    {
                        if (friend.IsFriendly(client.Account))
                        {
                            packet.Append(";" + player.Character.Level + ";");//FIXME
                            packet.Append(player.Character.Nickname).Append(";");
                            packet.Append(player.Character.Level).Append(";");
                            packet.Append("-1;");//FIXME : Alignement
                            packet.Append(player.Character.Breed).Append(";");
                            packet.Append(player.Character.Gender).Append(";");
                            packet.Append(player.Character.Look).Append(";");
                        }
                        else
                        {
                            packet.Append(";?;");//FIXME
                            packet.Append(player.Character.Nickname).Append(";");
                            packet.Append("?;");
                            packet.Append("-1;");//FIXME : Alignement
                            packet.Append(player.Character.Breed).Append(";");
                            packet.Append(player.Character.Gender).Append(";");
                            packet.Append(player.Character.Look).Append(";");
                        }
                    }
                }
            }
            client.Send(packet.ToString());
        }

        public static void WarnConnection(this Network.Game.GameClient client)
        {
            foreach (var friend in client.Account.Infos.Friends)
            {
                if (friend.IsFriendly(client.Account))
                {
                    var player = World.GetClientByAccount(friend.Account.AccountId);
                    if (player != null)
                    {
                        player.Send("Im0143;" + client.Account.Username + "(<b><a href='asfunction:onHref,ShowPlayerPopupMenu,"
                            + client.Character.Nickname + "'>" + client.Character.Nickname + "</a></b>)");
                    }
                }
            }

        }

        public static void SavePosition(this Network.Game.GameClient client)
        {
            if (client.Character.Map != null)
            {
                client.Character.SaveMap = client.Character.Map.ID;
                client.Character.SaveCell = client.Character.CellID;
                client.Character.Save();
                client.Send("Im06;");
            }
        }

        public static void RefreshLook(this Network.Game.GameClient client)
        {
            if (client.Character.Map != null)
            {
                client.Character.Map.Send("Oa" + client.Character.ID + "|" + client.Character.Bag.DisplayBagOnCharacter);
            }
        }

        public static void RefreshPods(this Network.Game.GameClient client)
        {
            client.Send("Ow" + client.Character.Bag.BagWeight + "|" + client.Character.MaxPods);
        }
    }
}
