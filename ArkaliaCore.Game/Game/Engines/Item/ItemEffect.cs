using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Engines.Item
{
    public class ItemEffect
    {
        public Enums.EffectTypeEnum EffectID = Enums.EffectTypeEnum.None;
        public int Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public bool Randomble = true;

        private string _effect { get; set; }

        public ItemEffect(string effect)
        {
            this._effect = effect;
            this.load();
        }

        private void load()
        {
            var data = this._effect.Split('#');
            var statID = int.Parse(data[0], System.Globalization.NumberStyles.HexNumber);
            this.EffectID = (Enums.EffectTypeEnum)statID;

            if (Items.ItemManager.IsWeaponEffect(statID))//Chech if this effect can random
                Randomble = false;

            this.Min = int.Parse(data[1], System.Globalization.NumberStyles.HexNumber);
            if (data.Length > 2)
            {
                this.Max = int.Parse(data[2], System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                this.Max = 0;
            }
            if (data.Length > 4)
            {
                this.Value = int.Parse(data[4].Split('+')[1]);
            }
            else
            {
                this.Value = 0;
            }
        }

        private int getMinWithoutValue
        {
            get
            {
                return Min - Value < 0 ? 0 : Min - Value;
            }
        }

        private int getMaxWithoutValue
        {
            get
            {
                return Max - Value < 0 ? 0 : Max - Value;
            }
        }

        public void RandomThis(bool max = false)
        {
            //Generate the random value from the template effect
            var randomValue = 0;
            if (this.Min > this.Max)
            {
                randomValue = this.Min;
            }
            else
            {
                randomValue = Utilities.Basic.RandomNumber(this.Min, this.Max);
            }

            if (max)
            {
                randomValue = this.Max;
            }

            if (randomValue == 0)
            {
                randomValue = 1;
            }

            this.Min = randomValue;
            this.Max = 0;
            this.Value = randomValue;
        }

        public ItemEffect GetRandomEffect(bool max = false)
        {
            if (Randomble)
            {
                var e = new ItemEffect(this._effect);
                e.RandomThis(max);
                return e;
            }
            else
            {
                return new ItemEffect(this._effect);
            }
        }

        public override string ToString()
        {
            return this.EffectID.ToString("x").Replace("000000", "").ToLower() + "#" + (Min == 0 ? "" : Min.ToString("x")).ToLower() + "#" + Max.ToString("x").ToLower() +
                "#0#" + this.getMinWithoutValue.ToString("x").ToLower() + "d" + Value.ToString() + "+" + Value.ToString();
        }
    }
}
