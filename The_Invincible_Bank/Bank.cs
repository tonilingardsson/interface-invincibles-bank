using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Bank
    {
        static public List<User> UserAccounts { get; private set; }

        static private int currentUserAccount = -1;
        // Holds the all the transfers
        static private List<Transfer> ListOfTransfers = new List<Transfer>();
        public Bank()
        {
            var adminOne = new Admin(1111, "1111");//test
            var customerOne = new Customer(2222, "2222");
            var customertwo = new Customer(3333, "3333");
            customertwo.CreateBankAccount("Vacation savings", "SEK"); //Funktion finns inte ännu
            UserAccounts = new List<User>();

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
            //BankAccount senderAccount = GetBankAccountByNumber(senderAccountNumber);
            //BankAccount receiverAccount = GetBankAccountByNumber(receavingAccountNumber);

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
            UI.DisplayMessage("Transfer failed: Invalid sender or receiver account.", ConsoleColor.Red, ConsoleColor.Red);
            return false;

        }
        public void ProcessTransfers()
        {
            UI.DisplayMessage("\n=== Processing All Transfers ===");

            // Loop through each transfer in the list
            foreach (Transfer transfer in ListOfTransfers)
            {

                // 1. Withdraw from sender
                transfer.FromAccount.Withdraw(transfer.Amount);

                // 2. Deposit to receiver
                transfer.ToAccount.Deposit(transfer.Amount);

                // 3. Write transaction history to BOTH account files
                transfer.FromAccount.WriteToFile(
                    $"Transferred {transfer.Amount} {transfer.CurrencyType} to account {transfer.FromAccount.AccountNumber}"
                );

                transfer.ToAccount.WriteToFile(
                    $"Received {transfer.Amount} {transfer.CurrencyType} from account {transfer.ToAccount.AccountNumber}"
                );

                UI.DisplayMessage($"Transfer completed: {transfer}");


                // 4. Empty the list after processing all transfers
                ListOfTransfers.Clear();

                UI.DisplayMessage("=== All Transfers Processed ===\n");
            }
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
            if (CheckAccountBorrowValidity(sum))
            {
                //Add to transfer list
                if (bankAccount != null)
                {
                    bankAccount.Deposit(sum);
                }
                UI.DisplayMessage("An interest rate of 7% has been applied to your loan", ConsoleColor.DarkYellow, ConsoleColor.DarkYellow);
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

            if (totalWorth * 5 > sum) //Checks if the the worth is more or less than the borrow amount
            {
                return true;
            }
            return false;
        }
        private int UserLogIn() //If this returns -1, the user failed to log in within 3 tries
        {

            int inputSecurityNumber;
            bool accountExists = false;
            string inputPassword = string.Empty;
            int userIndex = 0;
            int userLoginTries = 0;

            UI.DisplayMessage("1: Log in\n2: Exit program");
            if (Input.GetNumberFromUser(1, 2) == 2)
            {
                return -1;
            }
            Console.Clear();

            UI.DisplayMessage("Security number: ");
            while (!int.TryParse(Console.ReadLine(), out inputSecurityNumber) && inputSecurityNumber.ToString().Length != 4)
            {
                UI.DisplayMessage("This is not a valid security number, please try again", ConsoleColor.Red, ConsoleColor.Red);
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
                        inputPassword = Console.ReadLine();
                        // ---

                        if (!user.LogIn(inputSecurityNumber, inputPassword))
                        {
                            Console.Clear();
                            UI.DisplayMessage("Wrong Password"); //Replace

                            userLoginTries++;
                            if (userLoginTries == 3)
                            {
                                UI.DisplayMessage("You have failed to enter the right credentials too many times.\nClosing system....", ConsoleColor.Red, ConsoleColor.Red);
                                return -1;
                            }
                        }
                    }
                    userIndex = UserAccounts.IndexOf(user);
                    accountExists = true;
                    break;
                }
            }
            if (!accountExists)
            {
                UI.DisplayMessage("This account does not exist in our bank.\nExit program or try again?\nExit program: 1 | Try again: 2");

                if (Input.GetNumberFromUser(1, 2) == 2)
                {
                    UserLogIn();
                }
                else
                {
                    userIndex = -1;
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
            bool exist = false;

            while (!exist)
            {
                currentUserAccount = UserLogIn(); //Logs in to user and sets the current user index
                if (currentUserAccount == -1) //The user failed to login within 3 tries. 
                {
                    return false;
                }
                if (UserAccounts[currentUserAccount] is Customer)
                {
                    var customer = UserAccounts[currentUserAccount] as Customer;
                    Menu.CustomerMenu(customer);
                    currentUserAccount = -2;
                }
                else if (UserAccounts[currentUserAccount] is Admin)
                {
                    var admin = UserAccounts[currentUserAccount] as Admin;
                    Menu.AdminMenu(admin);
                    currentUserAccount = -2;
                }
            }


            return true;
        }
    }
}
