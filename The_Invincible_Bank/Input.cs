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
            Console.WriteLine();
            Console.Write("Your choice: ");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
            {
                Console.WriteLine($"You have to choose between {min} and {max}");
            }
            return choice;
        }
        
        public static decimal GetDecimalFromUser(BankAccount fromAccount)
        {
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <=0 || fromAccount.Sum < amount)
            {
                if (amount <=0) 
                {
                    Console.WriteLine($"The amount you want to send must be above zero ");
                }
                else if(fromAccount.Sum < amount)
                {
                    Console.WriteLine($"You do not have enough funds to send {amount:C}");
                }
            }
            return amount;
        }

    }
}
