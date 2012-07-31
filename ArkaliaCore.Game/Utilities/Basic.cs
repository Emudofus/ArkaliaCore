using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Utilities
{
    public static class Basic
    {
        public static Random Rand = new Random(Environment.TickCount);
        public static string[] LettersPairs = { "lo", "la", "li", "wo", "wi", "ka", "ko", "ki", "po",
                                                  "pi", "pa", "aw", "al", "na", "ni", "ny", "no", "ba", "bi",
                                                  "ra", "ri", "ze", "za", "da", "zel", "wo", "-" };

        public static int RandomNumber(int min, int max)
        {
            return Rand.Next(min, max);
        }

        public static string GetRandomName()
        {
            string Name = "";
            for (int i = 0; i <= RandomNumber(2, 4); i++)
            {
                Name += LettersPairs[RandomNumber(0, LettersPairs.Length - 1)];
            }
            Name = Name.ToCharArray()[0].ToString().ToUpper() + Name.Substring(1);
            return Name;
        }

        public static string GetUptime()
        {

            long Uptime = Environment.TickCount - Definitions.StartTime;
            string Time = "";

            if (Uptime >= 24 * 60 * 60 * 1000)
            {
                int Jours = 0;
                while (Uptime > 24 * 60 * 60 * 1000)
                {
                    Jours += 1;
                    Uptime -= 24 * 60 * 60 * 1000;
                }
                Time += Jours + "day" + (Jours > 1 ? "s" : "") + " ";
            }
            if (Uptime >= 60 * 60 * 1000)
            {
                int Hours = 0;
                while (Uptime > 60 * 60 * 1000)
                {
                    Hours += 1;
                    Uptime -= 60 * 60 * 1000;
                }
                Time += Hours + "h ";
            }
            if (Uptime >= 60 * 1000)
            {
                int Minutes = 0;
                while (Uptime > 60 * 1000)
                {
                    Minutes += 1;
                    Uptime -= 60 * 1000;
                }
                Time += Minutes + "m ";
            }
            if (Uptime >= 1000)
            {
                int Seconds = 0;
                while (Uptime > 1000)
                {
                    Seconds += 1;
                    Uptime -= 1000;
                }
                Time += Seconds + "s ";
            }

            int Millisecs = 0;
            while (Uptime > 0)
            {
                Millisecs += 1;
                Uptime -= 1;
            }
            Time += Millisecs + "ms";

            return Time;

        }
    }
}
