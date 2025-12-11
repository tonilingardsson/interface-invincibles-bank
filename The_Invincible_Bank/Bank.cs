using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Bank
    {
        static public List<User> UserAccounts { get; private set; }
        static private List<Transfer> ListOfTransfers = new List<Transfer>(); // Holds the all the transfers/test
        static public List<Customer> LockedCustomerAccounts { get; private set; } // Prision for customers who dont remeber their password >:3 
        static private int currentUserAccount = -1;

        static public DateTime lastRunTime = DateTime.Now;
        static public readonly TimeSpan interval = TimeSpan.FromSeconds(40);

        public Bank()
        {

            var adminOne = new Admin(1111, "1111");//test
            var customerOne = new Customer(2222, "2222");
            var customertwo = new Customer(3333, "3333");
            customertwo.CreateBankAccount("Vacation savings", "SEK"); //Funktion finns inte ännu
            UserAccounts = new List<User>();
            LockedCustomerAccounts = new List<Customer>();

            UserAccounts.Add(adminOne);
            UserAccounts.Add(customerOne);
            UserAccounts.Add(customertwo);
        }

        public void AddUserToList(User user)
        {
            UserAccounts.Add(user);
        }

        static public BankAccount? GetBankAccountByNumber(string bankAccountNumber)
        {
            foreach (var account in UserAccounts) // Steps in to the list of accounts
            {
                if (account is Customer)
                {
                    var customer = (Customer)account;
                    foreach (BankAccount bankAccount in customer.Accounts) //Steps in to the list of accounts the user own
                    {
                        if (bankAccount.AccountNumber == bankAccountNumber)
                        {
                            return bankAccount;
                        }
                    }
                }
            }
            return null; //Returns null if the account does not exist
        }

        public static bool Transfer(BankAccount senderAccount, BankAccount receiverAccount, decimal sum)
        {

            if (senderAccount != null && receiverAccount != null && CheckFounds(senderAccount, sum) && CheckIfOwnerOfThisAccount(senderAccount))
            {
                //  Get currency types from both accounts
                string fromCurrency = senderAccount.CurrencyType.ToString();
                string toCurrency = receiverAccount.CurrencyType.ToString();

                //  Create a transfer object
                Transfer newTransfer = new Transfer(
                    senderAccount,
                    receiverAccount,
                    sum
                );

                //Add it to the list of transfers
                ListOfTransfers.Add(newTransfer);
                UI.DisplayMessage("A sum of " + sum + " " + senderAccount.CurrencyType + " has been tranfered from account: " + senderAccount.AccountNumber + " to account: " + receiverAccount.AccountNumber, ConsoleColor.Green, ConsoleColor.Green);
                return true;
            }
            UI.DisplayMessage("Transfer failed: Insufficient founds or invalid receiver account.", ConsoleColor.Red, ConsoleColor.Red);
            return false;
        }
        static public void ProcessTransfers()
        {

            // Loop through each transfer in the list
            foreach (Transfer transfer in ListOfTransfers)
            {

                // 1. Withdraw from sender
                transfer.FromAccount.Withdraw(transfer.FromAmount);

                // 2. Deposit to receiver
                transfer.ToAccount.Deposit(transfer.ToAmount);

                // 3. Write transaction history to BOTH account files
                transfer.FromAccount.WriteToFile(
                    $"Transferred {transfer.FromAmount} {transfer.FromAccount.CurrencyType} to account {transfer.FromAccount.AccountNumber}"
                );

                transfer.ToAccount.WriteToFile(
                    $"Received {transfer.ToAmount} {transfer.ToAccount.CurrencyType} from account {transfer.ToAccount.AccountNumber}"
                );

                UI.DisplayMessage("All queued transferes processed", ConsoleColor.DarkGreen, ConsoleColor.DarkGreen);
            }
                // 4. Empty the list after processing all transfers
                ListOfTransfers.Clear();
        }

        static private bool CheckFounds(BankAccount senderAccount, decimal sum)
        {
            if (senderAccount.Sum >= sum)
            {
                return true;
            }
            return false; //Returns false if the account does not have enough money on it
        }

        static public bool CheckIfOwnerOfThisAccount(BankAccount senderAccount)
        {
            var customer = UserAccounts[currentUserAccount] as Customer;

            foreach (BankAccount account in customer.Accounts) // Steps in to the list of accounts
            {
                if (senderAccount == account)
                {
                    return true;
                }
            }
            return false; //Returns false if the account is not owned by the sender.
        }

        public static bool Borrow(BankAccount bankAccount, decimal sum)
        {
            decimal interest = sum * 0.07m;
            decimal totalRepayment = interest + sum;
            if (CheckAccountBorrowValidity(sum))
            {
                //Add to transfer list
                if (bankAccount != null)
                {
                    bankAccount.Deposit(sum);
                }
                UI.DisplayMessage("An interest rate of 7% has been applied to your loan\n" +
                    "Total to pay back is "+ totalRepayment +" "+bankAccount.CurrencyType+"", ConsoleColor.DarkYellow, ConsoleColor.DarkYellow);
                return true;
            }

            return false;
        }
        private static bool CheckAccountBorrowValidity(decimal sum)
        {
            decimal totalWorth = 0;

            if (UserAccounts[currentUserAccount] is Customer customer)
            {
                foreach (var account in customer.Accounts) //Gets the worth of all the accounts of the current user
                {
                    totalWorth += account.Sum;
                }
            }

            if (totalWorth * 5 >= sum) //Checks if the the worth is more or less than the borrow amount
            {
                return true;
            }
            return false;
        }
        private int UserLogIn() //Returns -1 to exit program
        {

            int inputSecurityNumber;
            bool accountExists = false;
            string inputPassword = string.Empty;
            int userIndex = 0;
            int userLoginTries = 0;
            bool tryAgain = true;

            UI.DisplayMessage("1: Log in\n2: Exit program");
            if (Input.GetNumberFromUser(1, 2) == 2)
            {
                return -1;
            }
            Console.Clear();

            while (tryAgain)
            {

                UI.DisplayMessage("Security number: ");
                while (!int.TryParse(Console.ReadLine(), out inputSecurityNumber) && inputSecurityNumber.ToString().Length != 4)
                {
                    UI.DisplayMessage("This is not a valid security number, please try again", ConsoleColor.Red, ConsoleColor.Red);
                }
                //Checks if the account exists in the locked out customer list. 
                foreach (var user in LockedCustomerAccounts)
                {
                    if (user.SecurityNumber == inputSecurityNumber)
                    {
                        UI.DisplayMessage("This account is locked out of our system. Please contact Admin for support.", ConsoleColor.Red, ConsoleColor.Red);
                        return -2;
                    }
                }
                //check if account exists in the user account list
                foreach (var user in UserAccounts)
                {
                    if (user.SecurityNumber == inputSecurityNumber) //If we found the security number in the list of users
                    {
                        while (!user.LogIn(inputSecurityNumber, inputPassword)) //As long as the password is wrong
                        {
                            UI.DisplayMessage("Password: "); //Replace

                            // ---
                            inputPassword = UI.HidePassword();
                            // ---

                            if (!user.LogIn(inputSecurityNumber, inputPassword))
                            {
                                Console.Clear();
                                UI.DisplayMessage("Wrong Password", ConsoleColor.Red, ConsoleColor.Red); //Replace

                                userLoginTries++;
                                if (userLoginTries == 3)
                                {
                                    if (user is Customer customer)
                                    {
                                        UI.DisplayMessage("You have failed to enter the right credentials too many times.\nThis account is locked out of our system. Contact admin for support.", ConsoleColor.Red, ConsoleColor.Red);
                                        LockedCustomerAccounts.Add(customer);
                                        return -2;
                                        tryAgain = false;
                                    }
                                }
                            }
                        }
                        userIndex = UserAccounts.IndexOf(user);
                        accountExists = true;
                        tryAgain = false;
                        break;
                    }
                }
                if (!accountExists)
                {
                    UI.DisplayMessage("This account does not exist in our bank.\nExit program or try again?\nExit program: 1 | Try again: 2");

                    if (Input.GetNumberFromUser(1, 2) == 2)
                    {
                        tryAgain = true;
                        Console.Clear();
                    }
                    else
                    {
                        userIndex = -1;
                        tryAgain = false;
                    }
                }
            }   
            return userIndex; //Returns index of the user account that is logged in
        }

        public int GetAccount()
        {
            int getAccount = Input.GetNumberFromUser(999, 10000);
            return getAccount;
        }

        public bool Run()
        {
            //Koden börjar och slutar här.
            UI.DisplayLoggo();
            bool exit = false;

            while (!exit)
            {
                currentUserAccount = UserLogIn();
     
                if (currentUserAccount == -1)
                {
                    exit = true;
                }
                if (currentUserAccount > -1)
                {
                    if (UserAccounts[currentUserAccount] is Customer customer)
                    {
                        Menu.CustomerMenu(customer);
                        currentUserAccount = -2;
                    }
                    else if (UserAccounts[currentUserAccount] is Admin admin)
                    {
                        Menu.AdminMenu(admin);
                        currentUserAccount = -2;
                    }
                }
            }
            return true;
        }
    }
}
