using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    static class Input
    {
        public static int GetNumberFromUser(int min, int max)
        {
            int choice;
           
            UI.DisplayMessage("Your choice: ");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
            {
                UI.DisplayMessage($"You have to choose between {min} and {max}");
            }
            return choice;
        }
        
        public static decimal GetDecimalFromUser()
        {
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <=0)
            {
                if (amount <=0) 
                {
                    UI.DisplayMessage($"The amount you want to send must be above zero!");
                }

            }
            return amount;
        }

    }
}
