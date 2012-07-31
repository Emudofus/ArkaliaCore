using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Candy.Core.IO;

namespace ArkaliaCore.Realm.Utilities
{
    public static class Settings
    {
        private static IniSettings m_settings { get; set; }

        public static void Load()
        {
            try
            {
                Logger.Infos("Load @configuration@ ...");
                m_settings = new IniSettings("Settings.ini");
                m_settings.ReadSettings();
                Logger.Infos("Configuration @loaded@ !");
            }
            catch (Exception e)
            {
                Logger.Error("Can't @found configuration@ !");
            }
        }

        public static IniSettings GetSettings
        {
            get
            {
                return m_settings;
            }
        }
    }
}
