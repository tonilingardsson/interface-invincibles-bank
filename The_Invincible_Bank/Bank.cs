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

        private int currentUserAccount = -1;
        // Holds the all the transfers
        private List<Transfer> ListOfTransfers = new List<Transfer>();
        public Bank()
        {
            var adminOne = new Admin(1111, "1111");//test
            var customerOne = new Customer(2222, "2222");
            UserAccounts = new List<User>();

            UserAccounts.Add(adminOne);
            UserAccounts.Add(customerOne);
        }

        public void AddUserToList(User user)
        {
            UserAccounts.Add(user);
        }
        public bool Transfer(string senderAccountNumber, string receavingAccountNumber, decimal sum)
        {
            BankAccount senderAccount = CheckSenderAccountValidity(senderAccountNumber, sum);
            BankAccount receiverAccount = CheckReceaverAccountValidity(receavingAccountNumber);

            if (senderAccount != null && receiverAccount != null)
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
                return true;
            }
            UI.DisplayMessage("Transfer failed: Invalid sender or receiver account.");
            return false;

        }
        private BankAccount FindBankAccountByNumber(string accountNumber)
        {
            foreach (User user in UserAccounts)
            {
                if (user is Customer customer)
                {
                    foreach (BankAccount account in customer.Accounts)
                    {
                        if (account.AccountNumber == accountNumber)
                        {
                            return account;
                        }
                    }
                }
            }
            return null;
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

        private BankAccount? CheckSenderAccountValidity(string senderAccountNumber, decimal sum)
        {
            if (UserAccounts[currentUserAccount] is Customer customer)
            {
                foreach (var account in customer.Accounts) //Gets the worth of all the accounts of the current user
                {
                    if (account.AccountNumber == senderAccountNumber && account.Sum >= sum)
                    {
                        return account;
                    }
                }
            }
            return null; //Retunerar null ifall kontot antingen inte finns eller inte har tillräckligt mycket pengar.
        }

        private BankAccount? CheckReceaverAccountValidity(string receavingAccountNumber)
        {
            foreach (Customer customerAccount in UserAccounts) // Steps in to the list of accounts
            {
                foreach (BankAccount bankAccount in customerAccount.Accounts) //Steps in to the list of accounts the user own
                {
                    if (bankAccount.AccountNumber == receavingAccountNumber)
                    {
                        return bankAccount;
                    }
                }
            }
            return null; //Retunrar null om kontot inte finns.
        }

        public bool Borrow(int bankAccount, decimal sum)
        {
            if (CheckAccountBorrowValidity(sum))
            {
                //Add to transfer list
                UI.DisplayMessage("The amount of " + sum + " will be transfered to your account momentarely.\nAn interest of 7% has been applied");
                return true;
            }

            return false;
        }
        private bool CheckAccountBorrowValidity(decimal sum)
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
                UI.DisplayMessage("This is not a valid security number, please try again");
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
                                UI.DisplayMessage("You have failed to enter the right credentials too many times.\nClosing system....");
                                return -1;
                            }
                        }
                    }
                    Console.WriteLine("Welcome!"); //Replace
                    userIndex = UserAccounts.IndexOf(user);
                    accountExists = true;
                    break;
                }
            }
            if (!accountExists)
            {
                UI.DisplayMessage("This account does not exist in our bank.\nTry again or exit program?\nTry again: 1 | Exit program: 2");

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
                    UserInterface.CustomerMenu(customer);
                    currentUserAccount = -2;
                }
                else if (UserAccounts[currentUserAccount] is Admin)
                {
                    var admin = UserAccounts[currentUserAccount] as Admin;
                    UserInterface.AdminMenu(admin);
                    currentUserAccount = -2;
                }
            }


            return true;
        }
    }
}
