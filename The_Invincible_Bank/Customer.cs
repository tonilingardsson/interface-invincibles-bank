using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            int number;
            string newSecurityNumber = string.Empty;

            for (int i = 0; i < 4; i++)
            {
                number = new Random().Next(0, 10);
                newSecurityNumber += number;
            }
            
            accountOne = new BankAccount("Bank Account", "SEK", newSecurityNumber);
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

        public void TransferMoney()
        {
            UI.DisplayMessage("1: Which account do you want to withdraw from?");
            BankAccount senderAccount = Bank.GetBankAccountByNumber(Input.GetAccountNumberFromUser());
            Console.Clear();
            UI.DisplayMessage("2: Which account do you want to deposit to?");
            BankAccount ReceaverAccount = Bank.GetBankAccountByNumber(Input.GetAccountNumberFromUser());
            Console.Clear();
            UI.DisplayMessage("How much money do you want to send?");
            decimal amount = Input.GetDecimalFromUser();
            Console.Clear();
            Bank.Transfer(senderAccount, ReceaverAccount, amount);
        }

        public void BorrowMoney()
        {
            ShowAccounts();
            UI.DisplayMessage("1 Which account do you want the money to be deposited to?");
            BankAccount account = Bank.GetBankAccountByNumber(Input.GetAccountNumberFromUser());
            if (account == null || !Bank.CheckIfOwnerOfThisAccount(account))
            {
                UI.DisplayMessage("The account you entered isn't owned by you or doesnt exist in our system", ConsoleColor.Red, ConsoleColor.Red);
            }
            else 
            {
                Console.Clear();
                UI.DisplayMessage("2: How much money do you want to borrow?");
                decimal amount = Input.GetDecimalFromUser();
                if (!Bank.Borrow(account, amount))
                {
                    UI.DisplayMessage("Transfer was not succesfull\nYour account did not qualify for borrowing this amount of money", ConsoleColor.Red, ConsoleColor.Red);
                }
                else
                {
                    UI.DisplayMessage("The amount of " + amount + " " + account.CurrencyType + " was transfered to your account.", ConsoleColor.Green, ConsoleColor.Green);
                }
            }
        }
        public void ShowAccountHistory(string bankAccount)
        {
BankAccount account = Bank.GetBankAccountByNumber(bankAccount);
            if (account == null)
            {
                UI.DisplayMessage("This account does not exsist", ConsoleColor.Red, ConsoleColor.Red);
            }
            else
            {
            UI.DisplayMessage("Transaction history for account:");
            UI.DisplayFile(Bank.GetBankAccountByNumber(bankAccount).FilePath);
        }
        }

        public void ConvertAccountCurrency(int bankAccount, string currencyToConvertTo)
        {
            //Konvertera om ett konto till den nya valutan igenom att
            //1: konvertera valutan
            //2: ändra valuta typ på kontot.
        }

        public void CreateBankAccount(string accountName, string currencyType)
        {
            //Creates and adds a new account to the account list. 
            {
                Random? rnd = new Random();
                string accountNumber = rnd.Next(0, 9999).ToString("D4");
                
                BankAccount newAccount = new BankAccount(accountName, currencyType, accountNumber);
                // Add it to the customer's account list
                Accounts.Add(newAccount);
            }
            //Make sure to generate a bank account number that does not already EXSIST IN THE LIST!
            //Användaren ska även få en ränta på sitt nya konto. 1%
        }
    }
}
