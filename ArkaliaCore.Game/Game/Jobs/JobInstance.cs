using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Jobs
{
    public class JobInstance
    {
        public Job LinkedJob { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }

        public JobInstance(Job job, int level)
        {
            this.LinkedJob = job;
            this.Level = level;
        }

        public void LevelUp(Network.Game.GameClient client)
        {
            if (Level + 1 <= 100)
            {
                client.Send("JN" + this.LinkedJob.ID + "|" + this.Level);
            }
        }

        public string GetBasicPattern
        {
            get
            {
                return this.LinkedJob.ID + ";" + this.Level + ";" + this.Exp + ";";
            }
        }

        public string GetAdvancedPattern
        {
            get
            {
            //    append(_template.getId()).append(";");
            //boolean first = true;
            //for(JobAction JA : _posActions)
            //{
            //    if(!first)str.append(",");
            //    else first = false;
            //    str.append(JA.getSkillID()).append("~").append(JA.getMin()).append("~");
            //    if(JA.isCraft())str.append("0~0~").append(JA.getChance());
            //    else str.append(JA.getMax()).append("~0~").append(JA.getTime());
            //}
                var pattern = new StringBuilder();
                pattern.Append(this.LinkedJob.ID).Append(";");
                var first = true;
                return pattern.ToString();
            }
        }

        public override string ToString()
        {
            return this.LinkedJob.ID + "," + this.Level + "," + this.Exp;
        }
    }
}
