using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ArkaliaCore.Game.Game.Chat
{
    public static class ChannelManager
    {
        private static Dictionary<string, Channels.Channel> m_channels = new Dictionary<string, Channels.Channel>();

        public static void Initialize()
        {
            Utilities.Logger.Infos("Initialize @channels@ ...");

            var types = Candy.Core.Reflection.TypesManager.GetTypes(typeof(Channels.Channel));
            foreach (var command in types)
            {
                try
                {
                    Channels.Channel instance = (Channels.Channel)Activator.CreateInstance(command);
                    m_channels.Add(instance.ChannelPrefix, instance);
                }
                catch { }
            }

            Utilities.Logger.Infos("@Channels@ initialized");
        }

        public static void RegisterChannel(string prefix, Channels.Channel channel)
        {
            lock (m_channels)
            {
                m_channels.Add(prefix, channel);
            }
        }

        public static Channels.Channel GetChannel(string prefix)
        {
            lock (m_channels)
            {
                if (m_channels.ContainsKey(prefix))
                {
                    return m_channels[prefix];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
