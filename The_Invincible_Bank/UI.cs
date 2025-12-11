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
        static public void DisplayMessage(string message, ConsoleColor frameColor = ConsoleColor.White, ConsoleColor textcolor = ConsoleColor.White)
        {
            message = message.Replace("\r", "");
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
            WriteColor("\n╔", frameColor);
            WriteColor(new string('═', maxStringLegnth + padding * 2), frameColor); //Creates a string containing ═ with the legnth of the longest string
            WriteColor("╗\n", frameColor);

            foreach (string line in lines)
            {
                WriteColor("║", frameColor);
                WriteColor(new string(' ', padding), frameColor); //Creates a new string with the legth of the padding
                WriteColor(line, textcolor);
                WriteColor(new string(' ', (maxStringLegnth - line.Length) + padding), frameColor); //adds more or less spaces to the right of the line depending on line legnth
                WriteColor("║\n", frameColor);
            }

            //Bottom row
            WriteColor("╚", frameColor);
            WriteColor(new string('═', maxStringLegnth + padding * 2), frameColor); //Creates a string containing ═ with the legnth of the longest string
            WriteColor("╝\n", frameColor);
        }
        static public void DisplayFile(string filename)
        {
            UI.DisplayMessage(File.ReadAllText(filename));
        }

        static private void WriteColor(string text, ConsoleColor color) //Just makes it easier for me to make the leggies on the player to switch colors
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        static public void WriteContinueMessage()
        {
            UI.DisplayMessage("Press any key to continue...", ConsoleColor.DarkGray, ConsoleColor.DarkGray);
            Console.ReadKey();
        }
    }
}
