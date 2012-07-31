using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace ArkaliaCore.Game.Utilities
{
    public class ConsoleStyle
    {
        public static string CurrentLoadingSymbol = "|";
        public static Timer LoadingSymbolTimer = new Timer(50);

        public static void InitConsole()
        {
            LoadingSymbolTimer.Elapsed += new ElapsedEventHandler(LoadingSymbolTimer_Elapsed);
        }

        public static void EnableLoadingSymbol()
        {
            LoadingSymbolTimer.Enabled = true;
            LoadingSymbolTimer.Start();
            Console.Write(" "  + CurrentLoadingSymbol);
        }

        public static void DisabledLoadingSymbol()
        {
            try
            {
                LoadingSymbolTimer.Enabled = false;
                LoadingSymbolTimer.Stop();
                LoadingSymbolTimer.Close();
                Console.CursorLeft -= 1;
                Console.Write(" ");
            }catch { }
        }

        private static void LoadingSymbolTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
            Console.CursorLeft -= 1;
            switch (CurrentLoadingSymbol)
            {
                case "|":
                    CurrentLoadingSymbol = "/";
                    break;

                case "/":
                    CurrentLoadingSymbol = "-";
                    break;

                case "-":
                    CurrentLoadingSymbol = "\\";
                    break;

                case "\\":
                    CurrentLoadingSymbol = "|";
                    break;
            }
            Console.Write(CurrentLoadingSymbol);
            }
            catch { }
        }
    }
}
