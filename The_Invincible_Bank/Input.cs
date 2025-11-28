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
        
        public static decimal GetDecimalFromUser()
        {
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <=0)
            {
                if (amount <=0) 
                {
                    Console.WriteLine($"The amount you want to send must be above zero ");
                }

            }
            return amount;
        }

        // Metod for att få accountName från user
        public static string GetString()
        {
            string accountName;
            Console.WriteLine(); // Ny råd
            UI.DisplayMessage("Enter account name: ");

            // Upprepa frågan tills user skicka nåt
            while(string.IsNullOrWhiteSpace(accountName = Console.ReadLine()))
            {
                UI.DisplayMessage("Account name cannot be empty. Please try again:");
            }

            return accountName;
        }

        // User väljer currency till kontot
        public static WorldMarket.Currency GetCurrency()
        {
            Console.WriteLine(); // Ny råd
            UI.DisplayMessage("Select currency:");
            UI.DisplayMessage("1: SEK (Swedish Krona)");  
            UI.DisplayMessage("2: USD (US Dollar)");            
            UI.DisplayMessage("3: EUR (Euro)");
            UI.DisplayMessage("4: GBP (British Pound)");

            // User måste välja ett av de 4 vål
            int currencyChoice = GetNumberFromUser(1,4);

            // Use the number to the correct Currency enum value
            switch (currencyChoice)
            {
                case 1: 
                return WorldMarket.Currency.Sek;
                case 2: 
                return WorldMarket.Currency.Dollar;
                case 3: 
                return WorldMarket.Currency.Euro;
                case 4: 
                return WorldMarket.Currency.Pound;
                // Fallback
                default: 
                return WorldMarket.Currency.Sek;
            }   
        }

    }
}
