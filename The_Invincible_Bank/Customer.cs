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
    internal class Customer : User
    {
        public List<BankAccount> Accounts { get; private set; }
        private BankAccount accountOne;

        // Ange kontot ett ID som kan räknas upp när man skapar nästa kontot
        private static int nextAccountNumber = 1000;

        public Customer(int securityNumber, string password)
            : base(securityNumber, password)
        {
            accountOne = new BankAccount("Bank Account", WorldMarket.Currency.Sek, nextAccountNumber++);
            Accounts = new List<BankAccount> { accountOne };
        }


        public void ShowAccounts() //Visa alla konton med hjälp av UI klassen plz
        {
            int countFrom = 1;
            foreach (var account in Accounts)
            {
                // Visa valutassymbol till varje konto
        string currencySymbol = account.CurrencyType switch
        {
            WorldMarket.Currency.Sek => "kr",
            WorldMarket.Currency.Dollar => "$",
            WorldMarket.Currency.Euro => "€",
            WorldMarket.Currency.Pound => "£",
            _ => ""
        };
        
                Console.WriteLine($"{countFrom}: {account.Name} - Balance: {account.Sum}{currencySymbol}");
                countFrom++;
            }
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
            // Sätter ett unik kontonummer genom nextAccountNumber
            int accountNumber = nextAccountNumber++; 
            // Skapar ett nytt konto
            BankAccount newAccount = new BankAccount(accountName, currencyType, accountNumber);
            // Lägger till kontot till kontolistan
            Accounts.Add(newAccount);
            // Konfirmationsmeddelande till user
            UI.DisplayMessage($"Account '{accountName}' created with number {accountNumber}");
            //Användaren ska även få en ränta på sitt nya konto. 1%
        }
    }
}
