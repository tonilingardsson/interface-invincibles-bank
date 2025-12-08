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
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                if (amount <= 0)
                {
                    UI.DisplayMessage($"The amount you want to send must be above zero!");
                }

            }
            return amount;
        }

        public static string GetAccountNumberFromUser()
        {

            UI.DisplayMessage("Please write a four digits account number: ");
            string account = Console.ReadLine();
            if (account.Length == 4)
            {
                foreach (char c in account)
                {
                    if (!char.IsDigit(c))
                    {
                        UI.DisplayMessage("It has to be digits!");
                        UI.WriteContinueMessage();
                        return GetAccountNumberFromUser();
                    }
                }
            }
            else
            {
                UI.DisplayMessage("The account must be 4 digits!");
                UI.WriteContinueMessage();
                return GetAccountNumberFromUser();
            }

            return account;
        }

        public static string GetString()
        {
            string accountName = null;
            while (accountName == null)
            {
                UI.DisplayMessage("Give this account a name: ");
                accountName = Console.ReadLine().Trim();

                if(accountName == null)
                {
                    UI.DisplayMessage("You must give it a name! It can't be empty or use space/s.");
                }
                else
                {
                    return accountName;
                }
            }
            return String.Empty;
        }

        public static string GetCurrency()
        {
            UI.DisplayMessage("Select currency:\n1: SEK\n2: USD\n3: EUR\n4: GBP");
            int choice = GetNumberFromUser(1,4);

            switch (choice)
            {
                case 1:
                return "SEK";
                
                case 2:
                return "USD";
                
                case 3:
                return "EUR";
                
                case 4:
                return "GBP";
            
                default:
                return "SEK";
            }
        }
    }
}