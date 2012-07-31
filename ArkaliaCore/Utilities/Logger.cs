using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Utilities
{
    public static class Logger
    {
        public static object Locker = new object();
        public static bool DebugMode = false;

        public static void DrawAscii()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"       ___       __        ___     ");
            Console.WriteLine(@"      / _ | ____/ /_____ _/ (_)__ _");
            Console.WriteLine(@"     / __ |/ __/  '_/ _ `/ / / _ `/ By NightWolf, only for Arkalia Server");
            Console.WriteLine(@"    /_/ |_/_/ /_/\_\\_,_/_/_/\_,_/  v" + Definitions.CoreVersion + ", Realm Core");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@" ______________________________________________________________________________");
            Console.WriteLine(@"");
        }

        public static void Append(string header, string message, ConsoleColor headcolor)
        {
            lock (Locker)
            {
                Console.ForegroundColor = headcolor;
                Console.Write(header);
                Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var c in message)
                {
                    if (c == '@')
                    {
                        if (Console.ForegroundColor == ConsoleColor.Gray)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                    else
                    {
                        Console.Write(c);
                    }
                }
                Console.Write("\n");
            }
        }

        public static void Infos(string message)
        {
            Append("[Infos]", message, ConsoleColor.Green);
        }

        public static void Error(string message)
        {
            Append("[Error]", message, ConsoleColor.Red);
        }

        public static void Debug(string message)
        {
            if (DebugMode)
            {
                Append("[Debug]", message, ConsoleColor.Magenta);
            }
        }

        public static void Warning(string message)
        {
            Append("[Warning]", message, ConsoleColor.Yellow);
        }
    }
}
