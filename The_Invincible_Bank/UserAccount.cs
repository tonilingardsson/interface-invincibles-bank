using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class UserAccount
    {
        private int SecurityNumber { get; set; }
        private string Password { get; set; }
        public List<BankAccount> Accounts { get; set; }

        public UserAccount(int securityNumber, string password)
        {
            SecurityNumber = securityNumber;
            Password = password;
        }

        public bool LogIn(int securityNumber, string password)
        {
            if (securityNumber == SecurityNumber)
            {
                if (password == Password)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Transfer()
        {
            Console.WriteLine("Do you want to transfer");
            while (Accounts.Count > 1)
            {
                int countFrom = 1;
                Console.WriteLine("Which account do you want to transfer from? ");
                foreach (var account in Accounts)
                {
                    Console.WriteLine($"{countFrom}: {account.Name} - Balance: {account.Sum:C}");
                    countFrom++;
                }

                var answer = new Input();

                bool hasMoney = false;
                BankAccount fromAccount = new BankAccount();
                int index = -1;
                while (!hasMoney)
                {
                    index = answer.GetNumberFromUser(1, Accounts.Count) - 1;
                    fromAccount = Accounts[index];
                    if (fromAccount.Sum <= 0)
                    {
                        Console.WriteLine("You have no funds to transfer. Please choose another account");
                    }
                    else
                    {
                        hasMoney = true;
                    }
                }

                Console.WriteLine("Which account do you want to send to? ");

                int countTo = 1;
                foreach (var account in Accounts)
                {
                    Console.WriteLine($"{countTo}: {account.Name} - Balance: {account.Sum:C}");
                    countTo++;
                }
                var receiver = new Input();


                int indexTo = receiver.GetNumberFromUser(1, Accounts.Count) - 1;
                BankAccount toAccount = Accounts[indexTo];
                while (index == indexTo)
                {
                    Console.WriteLine("You can't send to the same account");
                    indexTo = receiver.GetNumberFromUser(1, Accounts.Count) - 1;
                    toAccount = Accounts[indexTo];
                }

                decimal amount;
                Console.WriteLine("How much do you want to transfer? ");
                while (!decimal.TryParse(Console.ReadLine(), out amount) || amount > fromAccount.Sum)
                {
                    Console.WriteLine($"You dont have enough funds to transfer {amount:C}");
                }
                fromAccount.Sum -= amount;
                toAccount.Sum += amount;
                // Överför pengar mellan ett bankkonto till ett annat. 

                // Retunerar true om överföringen lyckades, annars false
                return true;
            }
            
        }

        public void ShowAccount()
        {
            //Visa alla konton med hjälp av UI klassen plz
        }

        public void ShowAccountHistory(int bankAccount)
        {
            //Visa kontots överföringshistorik. Vi kan använda oss av en textfil här
            //Använd dig av UI filen
        }

        public void ConvertAccountCurrency(int bankAccount, WorldMarket.Currency currencyToConvertTo)
        {
            //Konvertera om ett konto till den nya valutan igenom att
            //1: konvertera valutan
            //2: ändra valuta typ på kontot.
        }

        public void CreateBankAccount(string accountName, WorldMarket.Currency currencyType)
        {
            //Creates and adds a new account to the account list. 
            //Make sure to generate a bank account number that does not already EXSIST IN THE LIST!
            //Användaren ska även få en ränta på sitt nya konto. 1%
        }
    }
}
