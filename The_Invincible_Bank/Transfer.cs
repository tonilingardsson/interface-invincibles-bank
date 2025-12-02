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
        public decimal Amount { get; private set; }           // Mängden pengar (redan konverterad)
        public string CurrencyType { get; private set; }      // Valutan för mottagarkontot
        public DateTime Timestamp { get; private set; }       // När överföringen gjordes

        public Transfer(BankAccount fromAccount, BankAccount toAccount, decimal amount)
        {
            FromAccount = fromAccount;
            ToAccount = toAccount;

            // Här kopplar vi ihop med Currency klassen, konvertera beloppet till mottagarens valuta
            Amount = Currency.Convert(fromAccount.CurrencyType, toAccount.CurrencyType, amount);

            CurrencyType = toAccount.CurrencyType;
            Timestamp = DateTime.Now;

            // Uppdaterade saldon
            FromAccount.Withdraw(amount);
            ToAccount.Deposit(Amount);

            // Skriv transaktionen till båda kontonas filer
            string info = GetTransferInfo();
            FromAccount.WriteToFile($"Sent {amount} {fromAccount.CurrencyType} to account {ToAccount.AccountNumber}");
            ToAccount.WriteToFile($"Received {Amount} {CurrencyType} from account {FromAccount.AccountNumber}");
        }
        public string GetTransferInfo()
        {
           return $"{Timestamp}: {Amount} {CurrencyType} from {FromAccount.AccountNumber} to {ToAccount.AccountNumber}";
        }

    }
}
