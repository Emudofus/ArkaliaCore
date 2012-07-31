using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class BreedModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int StartLife { get; set; }
        public int StartAP { get; set; }
        public int StartMP { get; set; }
        public int StartInitiative { get; set; }
        public int StartProspection { get; set; }
        public string WeaponBonus { get; set; }
        public string Fire { get; set; }
        public string Water { get; set; }
        public string Agility { get; set; }
        public string Strenght { get; set; }
        public string Life { get; set; }
        public string Wisdom { get; set; }

        private Dictionary<Enums.StatsTypeEnum, List<Game.Stats.BreedFloor>> floors = 
                                new Dictionary<Enums.StatsTypeEnum, List<Game.Stats.BreedFloor>>();

        public void Load()
        {
            this.floors.Clear();
            this.floors.Add(Enums.StatsTypeEnum.LIFE, new List<Game.Stats.BreedFloor>());
            this.floors.Add(Enums.StatsTypeEnum.WISDOM, new List<Game.Stats.BreedFloor>());
            this.floors.Add(Enums.StatsTypeEnum.STRENGHT, new List<Game.Stats.BreedFloor>());
            this.floors.Add(Enums.StatsTypeEnum.FIRE, new List<Game.Stats.BreedFloor>());
            this.floors.Add(Enums.StatsTypeEnum.WATER, new List<Game.Stats.BreedFloor>());
            this.floors.Add(Enums.StatsTypeEnum.AGILITY, new List<Game.Stats.BreedFloor>());

            loadCaract(Enums.StatsTypeEnum.LIFE, Life);
            loadCaract(Enums.StatsTypeEnum.STRENGHT, Strenght);
            loadCaract(Enums.StatsTypeEnum.WISDOM, Wisdom);
            loadCaract(Enums.StatsTypeEnum.FIRE, Fire);
            loadCaract(Enums.StatsTypeEnum.WATER, Water);
            loadCaract(Enums.StatsTypeEnum.AGILITY, Agility);
        }

        private void loadCaract(Enums.StatsTypeEnum sType, string caract)
        {
            foreach (var floor in caract.Split('|'))
            {
                if (floor != "")
                {
                    var data = floor.Split(':');

                    var floorData = data[0].Split(',');
                    var costData = data[1].Split('-');

                    if (floorData.Length > 1)
                    {
                        this.floors[sType].Add(new Game.Stats.BreedFloor
                            (int.Parse(floorData[0]), int.Parse(floorData[1]), int.Parse(costData[0]), int.Parse(costData[1])));
                    }
                    else//Manual fix for life
                    {
                        this.floors[sType].Add(new Game.Stats.BreedFloor
                            (int.Parse(floorData[0]), int.MaxValue, int.Parse(costData[0]), int.Parse(costData[1])));
                    }
                }
            }
        }

        public Game.Stats.BreedFloor GetFloor(Enums.StatsTypeEnum sType, int floor)
        {
            Game.Stats.BreedFloor cost = null;
            foreach (var f in this.floors[sType])
            {
                if (f.From <= floor)
                {
                    cost = f;
                }
            }
            return cost;
        }

        public int GetCost(Enums.StatsTypeEnum sType, int floor)
        {
            int cost = 0;
            foreach (var f in this.floors[sType])
            {
                if (f.From <= floor)
                {
                    cost = f.Cost;
                }
            }
            return cost;
        }
    }
}
