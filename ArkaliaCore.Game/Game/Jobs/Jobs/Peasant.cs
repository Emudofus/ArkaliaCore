using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Jobs.Jobs
{
    public class Peasant : Job
    {
        public override int ID
        {
            get
            {
                return 28;
            }
        }

        public override int[] Resources
        {
            get
            {
                return new int[] { 47, 285, 389, 390, 396, 397, 399, 529, 530, 531, 534, 535, 
                    582, 583, 586, 587, 690, 2019, 2022, 2027, 2030, 2033, 2037, 6672, 427 };
            }
        }

        public override int[] Tools
        {
            get
            {
                return new int[1] { 577 }; ;
            }
        }
    }
}
