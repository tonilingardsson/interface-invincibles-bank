using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Bank
    {
        private List<Admin> adminAccounts;
        private List<Customer> customerAccounts;

        private Admin adminOne;
        private User userOne;

        private int currentUserAccount = -1;
        
        public Bank()
        {
            var adminOne = new Admin(1111, "1111");
            var userOne = new Customer(2222, "2222");

            adminAccounts = new List<Admin>();
            customerAccounts = new List<Customer>();

            adminAccounts.Add(adminOne);
            customerAccounts.Add(userOne);
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

        private bool CheckSenderAccountValidity(int accountOne, decimal sum)
        {
            
            foreach (BankAccount account in customerAccounts[currentUserAccount].Accounts) //Checks if the stated account is owned by the user
            {
                if (account.AccountNumber == accountOne)
                {
                    if (account.Sum >= sum) //If theres enough money on the account
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false; //Retunerar false ifall kontot antingen inte finns eller inte har tillräckligt mycket pengar.
        }
        private bool CheckReceaverAccountValidity(int accountTwo)
        {
            int counter = 0;
            foreach (Customer accountA in customerAccounts) // Steps in to the list of accounts
            {
                foreach (BankAccount accountB in customerAccounts[counter].Accounts) //Steps in to the list of accounts the user own
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

        public bool Borrow(int bankAccount, decimal amount)
        {
            // Låned användaren tar får inte överstiga värdet på ALLA användarens konton. 
            // Räntan skall vara på 7% 
            return true;
        }

        private void CreateNewUser()
        {
            int securityNumber = 0;
            string password = string.Empty;

            Console.WriteLine("Enter your security number. It should contain four digits"); //Replace
            while (!int.TryParse(Console.ReadLine(), out securityNumber) && securityNumber.ToString().Length != 4)
            {
                Console.WriteLine("Please enter a valid security number");
            }

            Console.WriteLine("Please enter a password");
            password = Console.ReadLine();

            Customer newAccount = new Customer(securityNumber, password);
            customerAccounts.Add(new Customer(securityNumber, password));

            Console.WriteLine("Account was created");
        }

        private int UserLogIn() //If this returns -1, the user failed to log in within 3 tries
        {

            int inputSecurityNumber;
            bool accountExists = false;
            string inputPassword = string.Empty;
            int userIndex = 0;
            int userLoginTries = 0;

            Console.Write("Security number: ");
            while (!int.TryParse(Console.ReadLine(), out inputSecurityNumber) && inputSecurityNumber.ToString().Length != 4)
            {
                Console.WriteLine("This is not a valid security number, please try again");
            }

            //check if account exists in the user account list
            foreach (var user in customerAccounts)
            {               
                if (user.SecurityNumber == inputSecurityNumber) //If we found the security number in the list of users
                {                   
                    while (!user.LogIn(inputSecurityNumber,inputPassword)) //As long as the password is wrong
                    {
                        Console.Write("Password: "); //Replace

                        // ---
                        inputPassword = Console.ReadLine();
                        // ---

                        if(!user.LogIn(inputSecurityNumber,inputPassword))
                        {
                            Console.Clear();
                            Console.WriteLine("Wrong Password"); //Replace

                            userLoginTries++;
                            if (userLoginTries == 3)
                            {
                                Console.WriteLine("You have failed to enter the right credentials too many times.");
                                Console.WriteLine("Closing system....");
                                return -1;
                            }
                        }
                    }
                    Console.WriteLine("Welcome!"); //Replace
                    accountExists = true;
                    break;                                   
                }
            }
            if (!accountExists)
            {
                Console.WriteLine("This account does not exist in our bank");
                Console.WriteLine("Create a new account or try again?");
                Console.WriteLine("New account: 1 | Try again: 2");

                if (Input.GetNumberFromUser(1, 2) == 1)
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
            //Logga
            //Välkommen
            currentUserAccount =  UserLogIn(); //Logs in to user and sets the current user index
            if (currentUserAccount == -1) //The user failed to login within 3 tries. 
            {
                return false;
            }
            return true;
        }
    }
}
