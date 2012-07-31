using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Engines
{
    public class EffectEngine
    {
        public string BaseEffect { get; set; }
        public List<Item.ItemEffect> Effects = new List<Item.ItemEffect>();

        public EffectEngine(string effects)
        {
            this.BaseEffect = effects;
        }

        public EffectEngine(List<Item.ItemEffect> effects)
        {
            this.Effects = effects;
        }

        public void Load()
        {
            var eSplitted = this.BaseEffect.Split(',');
            foreach (var e in eSplitted)
            {
                if (e != "")
                {
                    try
                    {
                        var eEffect = new Item.ItemEffect(e);
                        this.Effects.Add(eEffect);
                        Statistics.EffectLoadedCount++;//Statistic, useless
                    }
                    catch (Exception ex)
                    {
                        Utilities.Logger.Debug("Can't parse item effect : " + ex.ToString());
                    }
                }
            }
        }

        public List<Item.ItemEffect> GenerateEffects(bool max = false)
        {
            var effects = new List<Item.ItemEffect>();
            foreach (var e in this.Effects)
            {
                var generatedEffect = e.GetRandomEffect(max);
                effects.Add(generatedEffect);
                Utilities.Logger.Debug("Generated effect : " + generatedEffect.ToString());
            }
            return effects;
        }

        public override string ToString()
        {
            return string.Join(",", this.Effects);
        }
    }
}
