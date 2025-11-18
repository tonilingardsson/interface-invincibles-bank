using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Bank
    {
        List<UserAccount> UserAccounts { get; set; }
        Input input;
        int currentUserAccount = -1;
        
        public Bank()
        {
            UserAccounts = new List<UserAccount>();
            input = new Input();
        }

        public bool TransferBetweenAccounts(int accountOne, int accountTwo)
        {
            //Skicka över pengar från ett konto till ett annat mellan två OLIKA användare. 
            //Kom ihåg att converta valutan
            //Kom ihåg att kolla så att det finns tillräckligt med pengar, om inte, retunera false.
            bool validAccount = false;
            int counter = 0;

            //Checks if the stated account is owned by the user
            foreach (BankAccount account in UserAccounts[currentUserAccount].Accounts)
            {
                if (account.AccountNumber == accountOne)
                {
                    validAccount = true;
                    break;
                }
            }
            if (validAccount == true)
            {
                validAccount = false;

                foreach (UserAccount accountA in UserAccounts) // Steps in to the list of accounts
                {
                    foreach (BankAccount accountB in UserAccounts[counter].Accounts) //Steps in to the list of accounts the user have
                    {
                        if (accountB.AccountNumber == accountTwo)
                        {
                            validAccount = true;
                        }
                    }
                    counter++;
                }
            }
        }

        public bool Borrow(int bankAccount, decimal amount)
        {
            // Låned användaren tar får inte överstiga värdet på ALLA användarens konton. 
            // Räntan skall vara på 7% 
            return true;
        }

        public void CreateNewUser()
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

            UserAccount newAccount = new UserAccount(securityNumber, password);
            UserAccounts.Add(new UserAccount(securityNumber, password));

            Console.WriteLine("Account was created");
        }

        public int UserLogIn() //If this returns -1, the user failed to log in within 3 tries
        {

            int inputSecurityNumber = 0;
            bool accountExists = false;
            string inputPassword = string.Empty;
            int userIndex = -1;
            int userLoginTries = 0;

            Console.Write("Security number: ");
            while (!int.TryParse(Console.ReadLine(), out inputSecurityNumber) && inputSecurityNumber.ToString().Length != 4)
            {
                Console.WriteLine("This is not a valid security number, please try again");
            }

            //check if account exists
            foreach (var user in UserAccounts)
            {
                /*
                if (user.SecurityNumber == inputSecurityNumber)
                {                   
                    while (!user.LogIn(inputSecurityNumber,inputPassword))
                        {
                            Console.Write("Password: ") //Replace

                            // ---
                            string inputPassword = Console.ReadLine();
                            // ---

                            if(!user.LogIn(inputSecurityNumber,inputPassword))
                            {
                                Console.Clear();
                                Console.WriteLine("Wrong Password") //Replace

                                userLoginTries++;
                                if (userLoginTries == 3)
                                {
                                    Console.WriteLine("You have failed to enter the right credentials too many times.")
                                    Console.WriteLine("Closing system....")
                                    return -1;
                                }
                            }
                        }
                    Console.WriteLine("Welcome!") //Replace
                    accountExists = true;
                    break;                                   
                }
                */
            }
            if (!accountExists)
            {
                Console.WriteLine("This account does not exist in out bank");
                Console.WriteLine("Create a new account or try again?");
                Console.WriteLine("New account: 1 | Try again: 2");

                if (input.GetNumberFromUser(1, 2) == 1)
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
        public bool Run()
        {
            //Koden börjar och slutar här.
            //Logga
            //Välkommen
            UserLogIn();

            return true;
        }
    }
}
