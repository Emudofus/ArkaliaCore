using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Utilities
{
    public static class Formulas
    {
        public static int CalculateWaypointCost(Database.Models.MapModel from, Database.Models.MapModel to)
        {
            return (int)(10 * (Math.Abs(to.X - from.X)) + Math.Abs(to.Y - from.Y));
        }
    }
}
