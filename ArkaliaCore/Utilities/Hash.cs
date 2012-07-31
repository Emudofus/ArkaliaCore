using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Realm.Utilities
{
    public static class Hash
    {
        public static char[] HASH = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

        public static string CryptPass(string Key, string Password)
        {
            string _Crypted = "";

            for (int i = 0; i < Password.Length; i++)
            {
                char PPass = Password[i];
                char PKey = Key[i];

                int APass = (int)PPass / 16;

                int AKey = (int)PPass % 16;

                int ANB = (APass + (int)PKey) % HASH.Length;
                int ANB2 = (AKey + (int)PKey) % HASH.Length;

                _Crypted += HASH[ANB];
                _Crypted += HASH[ANB2];

            }
            return _Crypted;
        }

        public static string CryptIP(string IP)
        {
            string[] Splitted = IP.Split('.');
            string Encrypted = "";
            int Count = 0;
            for (int i = 0; i < 50; i++)
            {
                for (int o = 0; o < 50; o++)
                {
                    if (((i & 15) << 4 | o & 15) == int.Parse(Splitted[Count]))
                    {
                        char A = (char)(i + 48);
                        char B = (char)(o + 48);
                        Encrypted += A.ToString() + B.ToString();
                        i = 0;
                        o = 0;
                        Count++;
                        if (Count == 4)
                            return Encrypted;
                    }
                }
            }
            return "DD";
        }

        public static string CryptPort(int config_game_port)
        {
            char[] HASH = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
	            't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
	            'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};
            int P = config_game_port;
            string nbr64 = "";
            for (int a = 2; a >= 0; a--)
            {
                nbr64 += HASH[(int)(P / (Math.Pow(64, a)))];
                P = (int)(P % (int)(Math.Pow(64, a)));
            }
            return nbr64;
        }
    }
}
