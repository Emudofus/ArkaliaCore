using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Jobs
{
    public static class JobsManager
    {
        private static Dictionary<int, Job> m_jobs = new Dictionary<int, Job>();

        public static void Initialize()
        {
            Utilities.Logger.Infos("Initialize @jobs@ ...");

            var types = Candy.Core.Reflection.TypesManager.GetTypes(typeof(Job));
            foreach (var command in types)
            {
                try
                {
                    var instance = (Job)Activator.CreateInstance(command);
                    m_jobs.Add(instance.ID, instance);
                }
                catch { }
            }

            Utilities.Logger.Infos("@Jobs@ initialized");
        }

        public static Job GetJob(int id)
        {
            lock (m_jobs)
            {
                if (m_jobs.ContainsKey(id))
                {
                    return m_jobs[id];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
