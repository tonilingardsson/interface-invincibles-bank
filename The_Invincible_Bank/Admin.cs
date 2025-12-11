using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Admin : User
    {
        public Admin(int securityNumber, string password)
        : base(securityNumber, password)
        {
        }
        public void CreateNewUser()
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
            Bank.UserAccounts.Add(new Customer(securityNumber, password));

            UI.DisplayMessage("Account was created");
        }
        public void HandleLockedAccounts()
        {
            string accounts = string.Empty;
            int index = 1;
            accounts += "Locked accounts: \n\n";
            int element = 0;
            foreach (var account in Bank.LockedCustomerAccounts)
            {
                accounts += "Index: " + index + ": ";
                accounts += account.SecurityNumber;
                accounts += "\n";
                index ++;
            }

            UI.DisplayMessage("Enter index of account you wish to unlock, press 0 to go back to menu.");
            UI.DisplayMessage(accounts);

            element = Input.GetNumberFromUser(0, index - 1);

            if ((element > 0 && element <= Bank.LockedCustomerAccounts.Count()))
            {
                UI.DisplayMessage("Account " + Bank.LockedCustomerAccounts[element - 1].SecurityNumber + " has been reinstated.", ConsoleColor.Green, ConsoleColor.Green);
                Bank.LockedCustomerAccounts.RemoveAt(element - 1);
                UI.WriteContinueMessage();
            }
        }
    }
}
