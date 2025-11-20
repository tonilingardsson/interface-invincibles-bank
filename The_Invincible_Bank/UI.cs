using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    static class UI
    {
        static public void DisplayLoggo()
        {
            //Printa ut en logga
        }
        static public void DisplayMessage(string message)
        {
            // ---
            Console.Write("╔");
            for (int i = 0; i < message.Length + 6; i++) //Adding 6 for window margin
            {
                Console.Write("═");
            }
            Console.Write("╗");
            Console.WriteLine();
            // ---
            
            // ---
            Console.Write("║   ");

            Console.Write(message);

            Console.Write("   ║");
            // ---

            // ---
            Console.WriteLine();
            Console.Write("╚");
            for (int i = 0; i < message.Length + 6; i++) //Adding 6 for window margin
            {
                Console.Write("═");
            }
            Console.Write("╝");
        }
        static public void DisplayFile(string filename)
        {
           
        }
    }
}
