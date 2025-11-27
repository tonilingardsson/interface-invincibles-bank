using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    public class Transfer
    {

        public string FromAccountId { get; private set; }   // Kontonummer pengarna kommer ifrån
        public string ToAccountId { get; private set; }     // Kontonummer pengarna skall till
        public decimal Amount { get; private set; }         // Mängden pengar (redan konverterad)
        public string CurrencyType { get; private set; }    // Valutan för mottagarkontot
        public DateTime Timestamp { get; private set; }     // När överföringen gjordes

        public Transfer(string fromAccountId, string toAccountId, decimal amount,
                        string fromCurrency, string toCurrency)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;

            // Här kopplar vi ihop med Currency-klassen:
            Amount = Currency.Convert(fromCurrency, toCurrency, amount);

            CurrencyType = toCurrency;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Timestamp}: {Amount} {CurrencyType} från {FromAccountId} till {ToAccountId}";
        }
    }
}
