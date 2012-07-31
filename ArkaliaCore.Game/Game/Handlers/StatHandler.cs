using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class StatHandler
    {
        public static void HandleBoostStat(Network.Game.GameClient client, string packet)
        {
            var sType = (Enums.StatsTypeEnum)int.Parse(packet.Substring(2));

            var value = 0;
            var cost = client.Character.GetBreed.GetFloor(sType, client.Character.GetStat(sType, out value));

            if (cost != null)
            {
                if (cost.Cost <= client.Character.CaractPoint)
                {
                    switch (sType)
                    {
                        case Enums.StatsTypeEnum.LIFE:
                            client.Character.Life += cost.Value;
                            client.Character.CurrentLife += cost.Value;
                            break;

                        case Enums.StatsTypeEnum.WISDOM:
                            client.Character.Wisdom += cost.Value;
                            break;

                        case Enums.StatsTypeEnum.STRENGHT:
                            client.Character.Strenght += cost.Value;
                            break;

                        case Enums.StatsTypeEnum.FIRE:
                            client.Character.Fire += cost.Value;
                            break;

                        case Enums.StatsTypeEnum.WATER:
                            client.Character.Water += cost.Value;
                            break;

                        case Enums.StatsTypeEnum.AGILITY:
                            client.Character.Agility += cost.Value;
                            break;
                    }
                    client.Character.CaractPoint -= cost.Cost;
                    client.Character.UpdateStatsFields();
                    client.SendStats();
                }
            }
            else
            {
                Utilities.Logger.Error("Invalide sType ID when he want boost stats : " + sType);
            }
        }
    }
}
