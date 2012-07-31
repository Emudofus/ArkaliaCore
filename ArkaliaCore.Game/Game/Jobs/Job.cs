using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Jobs
{
    public class Job
    {
        public virtual int ID { get; set; }
        public virtual int[] Tools { get; set; }
        public virtual int[] Resources { get; set; }

        public bool SuitableTool(int id)
        {
            return this.Tools.ToList().Contains(id);
        }

        public bool CanUseResource(int id)
        {
            return this.Resources.ToList().Contains(id);
        }

        public void LoadFromString(string value)
        {
            var data = value.Split(',');
        }
    }
}
