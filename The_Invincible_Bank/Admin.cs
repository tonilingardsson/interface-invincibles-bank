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
        private void UpdateCurrencyValue()
        {

        }
    }
}
