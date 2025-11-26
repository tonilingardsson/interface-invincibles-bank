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

        public Customer(int securityNumber, string password)
            : base(securityNumber, password)
        {
            accountOne = new BankAccount("Bank Account", WorldMarket.Currency.Sek, 1234);
            Accounts = new List<BankAccount> { accountOne };
        }


        public void ShowAccounts() //Visa alla konton med hjälp av UI klassen plz
        {
            int countFrom = 1;
            foreach (var account in Accounts)
            {
                UI.DisplayMessage($"{countFrom}: {account.Name}({account.AccountNumber}) - Balance: {account.Sum:C}");
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
            //Creates and adds a new account to the account list. 
            //Make sure to generate a bank account number that does not already EXSIST IN THE LIST!
            //Användaren ska även få en ränta på sitt nya konto. 1%
        }
    }
}
