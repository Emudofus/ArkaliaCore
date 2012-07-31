using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Stats
{
    public class BasicStatistics
    {
        public Dictionary<Enums.StatsTypeEnum, StatField> Fields = new Dictionary<Enums.StatsTypeEnum, StatField>();

        public BasicStatistics()
        {
            this.Fields.Add(Enums.StatsTypeEnum.AP, new StatField());
            this.Fields.Add(Enums.StatsTypeEnum.MP, new StatField());
            this.Fields.Add(Enums.StatsTypeEnum.LIFE, new StatField());
            this.Fields.Add(Enums.StatsTypeEnum.WISDOM, new StatField());
            this.Fields.Add(Enums.StatsTypeEnum.STRENGHT, new StatField());
            this.Fields.Add(Enums.StatsTypeEnum.FIRE, new StatField());
            this.Fields.Add(Enums.StatsTypeEnum.WATER, new StatField());
            this.Fields.Add(Enums.StatsTypeEnum.AGILITY, new StatField());
        }

        public StatField GetField(Enums.StatsTypeEnum type)
        {
            lock (this.Fields)
            {
                return this.Fields[type];
            }
        }

        public void Clear()
        {
            lock (this.Fields)
            {
                foreach (var field in Fields)
                {
                    field.Value.Clear();
                }
            }
        }
    }
}
