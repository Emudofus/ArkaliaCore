using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ArkaliaCore.Game.Utilities
{
    public static class Logger
    {
        public static object Locker = new object();
        public static bool DebugMode = true;

        public static FileBuffer ErrorBuffer { get; set; }

        public static void Initialize()
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
            if (!Directory.Exists("Logs/Channels"))
                Directory.CreateDirectory("Logs/Channels");
            ErrorBuffer = new FileBuffer("Logs/Error_at_" + GetFormattedDate + ".txt");
        }

        public static string GetFormattedDate
        {
            get
            {
                return DateTime.Now.ToString().Replace(":", ".").Replace("\\", ".").Replace("/", ".");
            }
        }

        public static void DrawAscii()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"       ___       __        ___     ");
            Console.WriteLine(@"      / _ | ____/ /_____ _/ (_)__ _");
            Console.WriteLine(@"     / __ |/ __/  '_/ _ `/ / / _ `/ By NightWolf, only for Arkalia Server");
            Console.WriteLine(@"    /_/ |_/_/ /_/\_\\_,_/_/_/\_,_/  v" + Definitions.CoreVersion + ", Game Core");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@" ______________________________________________________________________________");
            Console.WriteLine(@"");
        }

        public static void Append(string header, string message, ConsoleColor headcolor, bool line = true)
        {
            lock (Locker)
            {
                if(line)
                    Console.Write("\n");

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
            }
        }

        public static void Infos(string message)
        {
            Append("[Infos]", message, ConsoleColor.Green);
        }

        public static void Error(string message)
        {
            Append("[Error]", message, ConsoleColor.Red);
            ErrorBuffer.WriteLine("[" + GetFormattedDate + "] : " + message);
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

        public static void Script(string message)
        {
            Append("[Script]", message, ConsoleColor.DarkGreen);
        }

        public static void Stage(string stage)
        {
            Console.Write("\n\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("                 ================ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(stage);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" ================ ");
            Console.Write("\n");
        }
    }
}
