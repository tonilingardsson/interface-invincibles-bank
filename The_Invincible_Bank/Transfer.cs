using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    public class Transfer
    {
        public BankAccount FromAccount { get; private set; }  // Kontonummer pengarna kommer ifrån
        public BankAccount ToAccount { get; private set; }    // Kontonummer pengarna skall till
        public decimal FromAmount { get; private set; }       // Money that will be taken from the "fromAccount"
        public decimal ToAmount { get; private set; }         // Money that will be added to the "ToAccount"
        public DateTime Timestamp { get; private set; }       // När överföringen gjordes

        public Transfer(BankAccount fromAccount, BankAccount toAccount, decimal amount)
        {
            FromAccount = fromAccount;
            ToAccount = toAccount;

            // Här kopplar vi ihop med Currency klassen, konvertera beloppet till mottagarens valuta
            FromAmount = amount;
            ToAmount = Currency.Convert(fromAccount.CurrencyType, toAccount.CurrencyType, amount);

            Timestamp = DateTime.Now;
        }
    }
}
