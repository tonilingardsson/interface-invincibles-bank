using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class UserAccount
    {
        public List<BankAccount> Accounts {  get; set; }
        public bool Transfer(int bankAccountOne, int bankAccountTwo, decimal sum)
        {
            // Överför pengar mellan ett bankkonto till ett annat. 

            // Retunerar true om överföringen lyckades, annars false
            return true;
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
