using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class Bank
    {
        List<UserAccount> UserAccounts { get; set; }

        public Bank()
        {
            UserAccounts = new List<UserAccount>();
        }

        public bool TransferBetweenAccounts(int accountOne, int amountTwo)
        {
            //Skicka över pengar från ett konto till ett annat mellan två OLIKA användare. 
            //Kom ihåg att converta valutan
            //Kom ihåg att kolla så att det finns tillräckligt med pengar, om inte, retunera false.

            return true;
        }

        public bool Borrow(int bankAccount, decimal amount)
        {
            // Låned användaren tar får inte överstiga värdet på ALLA användarens konton. 
            // Räntan skall vara på 7% 
            return true;
        }

        public bool Run()
        {
            //Koden börjar och slutar här.

            return true;
        }
    }
}
