using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    static class UI
    {
        private static int padding = 4;
        static public void DisplayLoggo()
        {

            Console.WriteLine("  _____ _            _            _            _ _     _        ____              _    ");
            Console.WriteLine(" |_   _| |__   ___  (_)_ ____   _(_)_ __   ___(_) |__ | | ___  | __ )  __ _ _ __ | | __");
            Console.WriteLine("   | | | '_ \\ / _ \\ | | '_ \\ \\ / / | '_ \\ / __| | '_ \\| |/ _ \\ |  _ \\ / _` | '_ \\| |/ /");
            Console.WriteLine("   | | | | | |  __/ | | | | \\ V /| | | | | (__| | |_) | |  __/ | |_) | (_| | | | |   < ");
            Console.WriteLine("   |_| |_| |_|\\___| |_|_| |_|\\_/ |_|_| |_|\\___|_|_.__/|_|\\___| |____/ \\__,_|_| |_|_|\\_\\");
            Console.WriteLine();

        }
        static public void DisplayMessage(string message)
        {
            string[] lines = message.Split('\n'); //Creates an array with strings

            //Finds the longest string in the array
            int maxStringLegnth = 0;
            foreach (string line in lines)
            {
                if (line.Length > maxStringLegnth)
                {
                    maxStringLegnth = line.Length;
                }
            }

            //Top row
            Console.Write("╔");
            Console.Write(new string('═', maxStringLegnth + padding * 2)); //Creates a string containing ═ with the legnth of the longest string
            Console.WriteLine("╗");

            foreach (string line in lines)
            {
                Console.Write("║");
                Console.Write(new string(' ', padding)); //Creates a new string with the legth of the padding
                Console.Write(line);
                Console.Write(new string(' ', (maxStringLegnth - line.Length) + padding)); //adds more or less spaces to the right of the line depending on line legnth
                Console.WriteLine("║");
            }

            //Bottom row
            Console.Write("╚");
            Console.Write(new string('═', maxStringLegnth + padding * 2)); //Creates a string containing ═ with the legnth of the longest string
            Console.WriteLine("╝");
        }
        static public void DisplayFile(string filename)
        {
           
        }
    }
}
