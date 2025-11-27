using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    public static class Currency
    {
        //public enum Currency
        //{
        //    Dollar,
        //    Euro,
        //    Sek,
        //    Pound
        //}

        private static Dictionary<string, decimal> CurrencyList = new Dictionary<string, decimal>
        {
            { "Sek", 1m },
            { "Dollar", 9.45m },
            { "Euro", 10.96m },
            { "Pound", 12.44m }
        };

        //public Currency()
        //{
        //    CurrencyList.Add(Currency.Dollar, 9.45m);
        //    CurrencyList.Add(Currency.Euro, 10.96m);
        //    CurrencyList.Add(Currency.Pound, 12.44m);
        //    CurrencyList.Add(Currency.Sek, 1m);
        //}

        // Metod för att konvertera en summa från en valuta till en annan
        public static decimal Convert(string currentCurrency, string newCurrency, decimal sum)
        {
            //Konvertera om summan till det nya formatet
            // Konvertera summan till SEK
            decimal inSek = sum * CurrencyList[currentCurrency];

            //Konvertera från SEK till den nya valutan
            decimal result = inSek / CurrencyList[newCurrency];
            return result;
        }

        // Metod för att uppdatera valutakurserna med slumpmässiga variationer
        public static void UpdateCurrencyValue()
        {
            //Uppdatera listan med random värden innom en rimlig range av dens standardvärde. 

            Random rnd = new Random();

            // Loopar igenom alla valutor utom SEK (som alltid är 1)
            foreach (var currency in new List<string> { "Sek", "Dollar", "Euro", "Pound"})
            {
                decimal currentValue = CurrencyList[currency];

                // Skapar en slupmässig faktor mellan -5% och +5%
                decimal changePercent = (decimal)(rnd.NextDouble() * 0.10 - 0.05);

                // Uppdaterar värdet med den slumpmässiga förrändringen
                CurrencyList[currency] = Math.Round(currentValue * (1 + changePercent), 2); // Avrundar till två decimaler

            }

        }

        // Hjälpmetod för att skriva ut aktuella valutakurser
        public static void ShowCurrencies()
        {
            Console.WriteLine("Aktuella valutakurser (relativt SEK):");
            foreach (var kvp in CurrencyList)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();
        }

    }
}
