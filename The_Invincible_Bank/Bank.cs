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
        private List<User> userAccounts;

        private int currentUserAccount = -1;
        
        public Bank()
        {
            var adminOne = new Admin(1111, "1111");
            var customerOne = new Customer(2222, "2222");
            userAccounts = new List<User>();

            userAccounts.Add(adminOne);
            userAccounts.Add(customerOne);
        }

        public bool Transfer(int accountOne, int accountTwo, decimal sum)
        {         
            if (CheckSenderAccountValidity(accountOne, sum))
            {
                if (CheckReceaverAccountValidity(accountTwo))
                {
                    //Convert
                }                
            }
            return false;
        }

        public bool CheckSenderAccountValidity(int accountOne, decimal sum)
        {
            if (userAccounts[currentUserAccount] is Customer customer)
            {
                foreach (var account in customer.Accounts) //Gets the worth of all the accounts of the current user
                {
                    if (account.AccountNumber == accountOne)
                    {
                        if (account.Sum >= sum) //If theres enough money on the account
                        {
                            return true;
                        }
                    }
                }
            }
            return false; //Retunerar false ifall kontot antingen inte finns eller inte har tillräckligt mycket pengar.
        }

        private bool CheckReceaverAccountValidity(int accountTwo)
        {
            bool validAccount = false;
            int counter = 0;
            foreach (Customer accountA in userAccounts) // Steps in to the list of accounts
            {
                foreach (BankAccount accountB in accountA.Accounts) //Steps in to the list of accounts the user own
                {
                    if (accountB.AccountNumber == accountTwo)
                    {
                        return true;
                    }
                }
                counter++;
            }
            return false; //Retunrar false om kontot inte finns.
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

            if (userAccounts[currentUserAccount] is Customer customer)
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
                        customer.CreateBankAccount(Input.GetString(), Input.GetCurrency());
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
            foreach (var user in userAccounts)
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
                    userIndex = userAccounts.IndexOf(user);
                    accountExists = true;
                    break;                                   
                }
            }
            if (!accountExists)
            {
                UI.DisplayMessage("This account does not exist in our bank.\nCreate a new account or try again?\nNew account: 1 | Try again: 2");

                if (Input.GetNumberFromUser(1, 2) == 2)
                {
                    UserLogIn();
                }
                else
                {
                    CreateNewUser();
                    UserLogIn();
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
                if (userAccounts[currentUserAccount] is Customer)
                {
                    customerMenu();
                    currentUserAccount = -2;
                }
                else if (userAccounts[currentUserAccount] is Admin)
                {
                    adminMenu();
                    currentUserAccount = -2;
                }
            }


            return true;
        }
    }
}
