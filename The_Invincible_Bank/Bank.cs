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
            var adminOne = new Admin(1111, "1111");
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
                    senderAccountNumber,
                    receavingAccountNumber,
                    sum,
                    fromCurrency,
                    toCurrency
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
                // Find the sender account
                BankAccount senderAccount = FindBankAccountByNumber(transfer.FromAccountId);

                // Find the receiver account
                BankAccount receiverAccount = FindBankAccountByNumber(transfer.ToAccountId);

                if (senderAccount != null && receiverAccount != null)
                {
                    // Check if sender has enough money
                    if (senderAccount.Sum >= transfer.Amount)
                    {
                        // 1. Withdraw from sender
                        senderAccount.Withdraw(transfer.Amount);

                        // 2. Deposit to receiver
                        receiverAccount.Deposit(transfer.Amount);

                        // 3. Write transaction history to BOTH account files
                        senderAccount.WriteToFile(
                            $"Transferred {transfer.Amount} {transfer.CurrencyType} to account {transfer.ToAccountId}"
                        );

                        receiverAccount.WriteToFile(
                            $"Received {transfer.Amount} {transfer.CurrencyType} from account {transfer.FromAccountId}"
                        );

                        UI.DisplayMessage($"Transfer completed: {transfer}");
                    }
                    else
                    {
                        UI.DisplayMessage($"Transfer failed: Insufficient funds in account {transfer.FromAccountId}");
                    }
                }
                else
                {
                    UI.DisplayMessage($"Transfer failed: Account not found");
                }
            }

            // 4. Empty the list after processing all transfers
            ListOfTransfers.Clear();

            UI.DisplayMessage("=== All Transfers Processed ===\n");
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
        private void CreateNewUser()
        {
            int securityNumber = 0;
            string password = string.Empty;

            UI.DisplayMessage("Enter your security number. It should contain four digits");

            while (!int.TryParse(Console.ReadLine(), out securityNumber) && securityNumber.ToString().Length != 4)
            {
                UI.DisplayMessage("Please enter a valid security number");
            }

            UI.DisplayMessage("Please enter a password");
            password = Console.ReadLine();

            Customer newAccount = new Customer(securityNumber, password);
            userAccounts.Add(new Customer(securityNumber, password));

            UI.DisplayMessage("Account was created");
        }

        private void adminMenu()
        {
            var admin = userAccounts[currentUserAccount] as Admin;
            bool exist = false;

            while (!exist)
            {
                UI.DisplayMessage("1: Create new user\n2: Update currency value\n3: Log out");

                switch (Input.GetNumberFromUser(1,3))
                {
                    case 1:
                        CreateNewUser();
                        break;
                    case 2:
                        //Currency.ShowCurrencyValues()
                        //Currency.UpdateCurrency();
                        //Currency.ShowCurrencyValues()
                        break;
                    case 3:
                        exist = true;
                        break;
                }
            }

        }
        private void customerMenu()
        {
            var customer = userAccounts[currentUserAccount] as Customer;
            bool exist = false;

            while (!exist)
            {
                UI.DisplayMessage("1: Show Accounts\n2: Cretae new account\n3: Transfer money \n4: Convert account currency\n5: Show account history\n6: Borrow money\n7: Log out");

                switch (Input.GetNumberFromUser(1, 7))
                {
                    case 1:
                        customer.ShowAccounts();

                        break;
                    case 2:
                        //customer.CreateBankAccount(Input.GetString(), Input.GetCurrency());
                        break;
                    case 3:
                        //customer.Transfer();
                        break;
                    case 4:
                        //customer.ConvertAccountCurrency(Input.getBankAccount(), Input.getCurrency())
                        break;
                    case 5:
                        customer.ShowAccounts();
                        //customer.ShowAccountHistory(Input.getBankAccount());
                        break;
                    case 6:
                        //Borrow(Input.getBankAccount(), Input.GetDecimalFromUser());
                        break;
                    case 7:
                        exist = true;
                        break;
                }
            }
        }
        private int UserLogIn() //If this returns -1, the user failed to log in within 3 tries
        {

            int inputSecurityNumber;
            bool accountExists = false;
            string inputPassword = string.Empty;
            int userIndex = 0;
            int userLoginTries = 0;

            UI.DisplayMessage("1: Log in\n2: Exit program");
            if (Input.GetNumberFromUser(1,2) == 2)
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
                    while (!user.LogIn(inputSecurityNumber,inputPassword)) //As long as the password is wrong
                    {
                        UI.DisplayMessage("Password: "); //Replace

                        // ---
                        inputPassword = Console.ReadLine();
                        // ---

                        if(!user.LogIn(inputSecurityNumber,inputPassword))
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
            int getAccount = Input.GetNumberFromUser(999,10000);
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
