using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Input
    {
        public int GetNumberFromUser(int min, int max)
        {
            int choice;
            Console.WriteLine();
            Console.Write("Ditt val: ");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
            {
                Console.WriteLine($"Du måste välja mellan valen {min} till {max}");
            }
            return choice;
        }
    }
}
