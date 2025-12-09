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
                UI.DisplayMessage($"{countFrom}: {account.Name}({account.AccountNumber}) - Balance: {Math.Round(account.Sum, 2)} " + account.CurrencyType);
                countFrom++;
            }
        }

        public void TransferMoney()
        {
            UI.DisplayMessage("1: Which account do you want to withdraw from?");
            ShowAccounts();
            BankAccount senderAccount = Bank.GetBankAccountByNumber(Input.GetAccountNumberFromUser());
            Console.Clear();
            UI.DisplayMessage("2: Which account do you want to deposit to?\n" +
                "You can also transfer to other customer accounts.");
            ShowAccounts();
            BankAccount receaverAccount = Bank.GetBankAccountByNumber(Input.GetAccountNumberFromUser());
            Console.Clear();
            UI.DisplayMessage("3: In the currency of the withdrawal account: " + senderAccount.CurrencyType + ", how much money do you want to transfer?\n" +
                " Current founds: " + Math.Round(senderAccount.Sum, 2));
            decimal amount = Input.GetDecimalFromUser();
            Console.Clear();
            Bank.Transfer(senderAccount, receaverAccount, amount);
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
                UI.DisplayMessage($"The highest amount you can borrow is {account.Sum * 5} {account.CurrencyType}.", ConsoleColor.Blue);
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
                UI.DisplayMessage("This account does not exist", ConsoleColor.Red, ConsoleColor.Red);
            }
            else
            {
                UI.DisplayMessage("Transaction history for account:");
                UI.DisplayFile(Bank.GetBankAccountByNumber(bankAccount).FilePath);
            }
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

        // Deposit money into bank account
        public void DepositMoney()
        {

            // Show users their accounts
            ShowAccounts();
            UI.DisplayMessage("Which account do you want to deposit to?");

            // Get account number from user
            string accountNumber = Input.GetAccountNumberFromUser();
            BankAccount account = Bank.GetBankAccountByNumber(accountNumber);

            // Validate the account exists and is owned by the user
            if (account == null)
            {
                UI.DisplayMessage("This account does not exist.", ConsoleColor.Red, ConsoleColor.Red);
                return;
            }

            if (!Bank.CheckIfOwnerOfThisAccount(account))
            {
                UI.DisplayMessage("You do not own this account!", ConsoleColor.Red, ConsoleColor.Red);
                return;
            }

            Console.Clear();

            // Get deposit amount from user
            UI.DisplayMessage("How much money do you want to deposit?");
            decimal amount = Input.GetDecimalFromUser();

            // Validate amount
            if (amount <= 0)
            {
                UI.DisplayMessage("Deposit amount must be greater than zero.", ConsoleColor.Red, ConsoleColor.Red);
                return;
            }

            Console.Clear();

            // Perform the deposit 
            account.Deposit(amount);

            // Show success message with new balance
            string symbol = GetCurrencySymbol(account.CurrencyType);
            UI.DisplayMessage($"Successfully desposited {amount:N2} {symbol} to account {account.Name} ({account.AccountNumber})", ConsoleColor.Green, ConsoleColor.Green);
            UI.DisplayMessage($"New Balance: {account.Sum:N2} {symbol}");
        }

        // Fix the lack of currencySymbol
        private string GetCurrencySymbol(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return string.Empty;

            var c = currencyCode.Trim().ToUpperInvariant();

            return c switch
            {
                "SEK" => "kr",
                "USD" => "$",
                "EUR" => "€",
                "GBP" => "£",
                _ => c + " "
            };
        }
    }
}
