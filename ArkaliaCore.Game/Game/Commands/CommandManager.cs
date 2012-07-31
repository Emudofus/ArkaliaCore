using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ArkaliaCore.Game.Game.Commands
{
    public class CommandManager
    {
        private static Dictionary<string, Command> m_commands = new Dictionary<string, Command>();

        public static void Initialize()
        {
            Utilities.Logger.Infos("Initialize @commands@ ...");
            var types = Candy.Core.Reflection.TypesManager.GetTypes(typeof(Command));
            foreach (var command in types)
            {
                try
                {
                    Command instance = (Command)Activator.CreateInstance(command);
                    if (instance.NeedLoaded)
                    {
                        m_commands.Add(instance.Prefix, instance);
                    }
                }
                catch { }
            }
            Utilities.Logger.Infos("@Commands@ initialized");
        }

        public static Command GetCommand(string prefix)
        {
            lock (m_commands)
            {
                if (m_commands.ContainsKey(prefix))
                {
                    return m_commands[prefix];
                }
                else
                {
                    return null; 
                }
            }
        }

        public static List<Command> Commands
        {
            get
            {
                return m_commands.Values.ToList();
            }
        }

        public static void Add(string key, Command command)
        {
            lock (m_commands)
            {
                if (!m_commands.ContainsKey(key))
                {
                    m_commands.Add(key, command);
                }
            }
        }
    }
}
