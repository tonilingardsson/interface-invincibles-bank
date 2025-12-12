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
            string accounts = string.Empty;
            foreach (var account in Accounts)
            {
                if (countFrom != 1)
                {
                    accounts += "\n";
                }
                accounts += $"{countFrom}: {account.Name} ({account.AccountNumber}) - Balance: {Math.Round(account.Sum, 2)} {account.CurrencyType} " +

                countFrom++;
            }
            UI.DisplayMessage(accounts);
        }

        public void TransferMoney()
        {
            BankAccount senderAccount = null;
            BankAccount receaverAccount = null;
            int userInput;
            string userInputTwo;
            bool exit = false;

            UI.DisplayMessage("Which account do you want to withdraw from?\n0: Exit");
            ShowAccounts();

            userInput = Input.GetNumberFromUser(0, Accounts.Count);

            if (userInput == 0)
            {
                exit = true;
            }
            if (!exit)
            {
                senderAccount = Accounts.ElementAt(userInput - 1);


                while (receaverAccount == null && !exit)
                {
                    Console.Clear();
                    UI.DisplayMessage("2: Which account do you want to deposit to?\nYou can also transfer to other customer accounts.\n0: Exit");
                    ShowAccounts();
                    userInputTwo = Console.ReadLine();

                    if (userInputTwo == "0") //Checks if the user wants to leave
                    {
                        exit = true;
                    }
                    else if (int.TryParse(userInputTwo, out userInput) && userInput > 0 && userInput <= Accounts.Count) //Checks if the user chose an index from the list
                    {
                        receaverAccount = Accounts.ElementAt(userInput - 1);
                    }
                    else if (userInputTwo.Length == 4 && (int.TryParse(userInputTwo, out userInput)) == true) //Checks if the user entered a bank account number
                    {
                        while ((receaverAccount = Bank.GetBankAccountByNumber(userInputTwo)) == null)
                        {
                            UI.DisplayMessage("Invalid account.\n1: Try again.\n2: Exit");

                            if (Input.GetNumberFromUser(1, 2) == 2)
                            {
                                exit = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        UI.DisplayMessage("You've not entered a valid input", ConsoleColor.Red, ConsoleColor.Red);
                        UI.WriteContinueMessage();
                    }
                }
                if (!exit)
                {
                    Console.Clear();
                    UI.DisplayMessage("3: In the currency of the withdrawal account: " + senderAccount.CurrencyType + ", how much money do you want to transfer?\nCurrent founds: " + Math.Round(senderAccount.Sum, 2) + " " + senderAccount.CurrencyType);
                    decimal amount = Input.GetDecimalFromUser();
                    Console.Clear();
                    Bank.Transfer(senderAccount, receaverAccount, amount);
                }
            }
        }

        public void BorrowMoney()
        {
            int userInput;
            bool exit = false;
            BankAccount account = null;

            UI.DisplayMessage("1 Which account do you want the money to be deposited to?\n0: Exit");
            ShowAccounts();

            userInput = Input.GetNumberFromUser(0, Accounts.Count);

            if (userInput == 0)
            {
                exit = true;
            }

            if (!exit)
            {
                account = Accounts.ElementAt(userInput - 1);
                Console.Clear();
                UI.DisplayMessage($"The highest amount you can loan is {account.Sum * 5} {account.CurrencyType}.", ConsoleColor.Blue);
                UI.DisplayMessage("2: How much money do you want to loan?");
                decimal amount = Input.GetDecimalFromUser();
                if (!Bank.Borrow(account, amount))
                {
                    UI.DisplayMessage("Transfer was not succesfull\nYour account did not qualify for loan this amount of money", ConsoleColor.Red, ConsoleColor.Red);
                }
                else
                {
                    UI.DisplayMessage("The amount of " + amount + " " + account.CurrencyType + " was transfered to your account.", ConsoleColor.Green, ConsoleColor.Green);
                    account.WriteToFile(
                    $"Loan: {amount} {account.CurrencyType}"
                    );
                }
            }
        }
        public void ShowAccountHistory()
        {
            int userInput;
            bool exit = false;
            BankAccount account = null;

            UI.DisplayMessage("Which acount do you want to show history for?\n0: Exit");
            ShowAccounts();

            userInput = Input.GetNumberFromUser(0, Accounts.Count);

            if (userInput == 0)
            {
                exit = true;
            }
            if (!exit)
            {
                account = Accounts.ElementAt(userInput - 1);
                UI.DisplayFile(account.FilePath);
            }
        }

        public void CreateBankAccount(string accountName, string currencyType, decimal balance, string accountNumber)
        {
            //Creates and adds a new account to the account list. 
            {
                BankAccount newAccount = new BankAccount(accountName, currencyType, accountNumber, balance);
                // Add it to the customer's account list
                Accounts.Add(newAccount);
            }
        }

        // Deposit money into bank account
        public void DepositMoney()
        {
            int userInput;
            bool exit = false;
            BankAccount account = null;

            UI.DisplayMessage("1 Which account do you want the money to be deposited to?\n0: Exit");
            ShowAccounts();

            userInput = Input.GetNumberFromUser(0, Accounts.Count);

            if (userInput == 0)
            {
                exit = true;
            }

            if (!exit)
            {
                account = Accounts.ElementAt(userInput - 1);
                Console.Clear();

                // Get deposit amount from user
                UI.DisplayMessage("How much money do you want to deposit?");
                decimal amount = Input.GetDecimalFromUser();

                // Validate amount
                if (amount <= 0)
                {
                    UI.DisplayMessage("Invalid amount.", ConsoleColor.Red, ConsoleColor.Red);
                    return;
                }

                Console.Clear();
                decimal interest = account.Sum * 0.05m;
                decimal yearlyInterest = interest + account.Sum;
                decimal threeYearsInterest = yearlyInterest * 3;
                // Perform the deposit 
                account.Deposit(amount);
                account.WriteToFile(
                    $"Deposit: {amount} {account.CurrencyType}"
                    );
                          
                // Show success message with new balance
                string symbol = GetCurrencySymbol(account.CurrencyType);
                UI.DisplayMessage($"Successfully desposited {amount:N2} {symbol} to account {account.Name} ({account.AccountNumber})" +
                   $"\n\nThe interest is 5%. Your account in three years: {threeYearsInterest:N2} {account.CurrencyType}", ConsoleColor.Green, ConsoleColor.Green);
                UI.DisplayMessage($"New Balance: {account.Sum:N2} {symbol}");
            }


        }
        public void WithdrawMoney()
        {
            // Ask the user which account to withdraw money from
            UI.DisplayMessage("Which account do you want to withdraw money from?\n0: Exit");
            ShowAccounts();

            int userInput = Input.GetNumberFromUser(0, Accounts.Count);

            if (userInput == 0)
            {
                return; // Exit directly
            }

            // Validate that the user selected a valid account
            if (userInput < 1 || userInput > Accounts.Count)
            {
                UI.DisplayMessage("Invalid account selection.", ConsoleColor.Red, ConsoleColor.Red);
                return;
            }

            BankAccount account = Accounts.ElementAt(userInput - 1);
            Console.Clear();

            // Get withdrawal amount from user
            UI.DisplayMessage("How much money do you want to withdraw?");
            decimal amount = Input.GetDecimalFromUser();

            // Validate amount
            if (amount <= 0)
            {
                UI.DisplayMessage("Invalid amount.", ConsoleColor.Red, ConsoleColor.Red);
                return;
            }

            // Check if there are enough funds in the account
            if (amount > account.Sum)
            {
                UI.DisplayMessage("Error: You can not withdraw more money than the account balence", ConsoleColor.Red, ConsoleColor.Red);

                // Log failed withdrawal attempts
                account.WriteToFile(
                    $"Failed withdraw attempt: {amount:N2} {account.CurrencyType} | Date: {DateTime.Now}"
                );
                return;
            }

            Console.Clear();

            // Perform the withdraw
            account.Withdraw(amount);
            account.WriteToFile(
                $"Withdraw: {amount:N2} {account.CurrencyType} | Date: {DateTime.Now}"
            );

            // Show success message with new balance
            string symbol = GetCurrencySymbol(account.CurrencyType);
            UI.DisplayMessage($"Successfully withdraw {amount:N2} {symbol} from account {account.Name} ({account.AccountNumber})", ConsoleColor.Green, ConsoleColor.Green);
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
