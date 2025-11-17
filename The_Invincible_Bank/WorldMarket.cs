using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    public class WorldMarket
    {
        public enum Currency
        {
            Dollar,
            Euro,
            Sek,
            Pound
        }

        Dictionary<Currency, decimal> CurrencyList = new Dictionary<Currency, decimal>();
        public WorldMarket()
        {
            CurrencyList.Add(Currency.Dollar, 9.45m);
            CurrencyList.Add(Currency.Euro, 10.96m);
            CurrencyList.Add(Currency.Pound, 12.44m);
            CurrencyList.Add(Currency.Sek, 1m);


        }

        public decimal Convert(Currency currentCurrency, Currency newCurrency, decimal sum)
        {
            //Konvertera om summan till det nya formatet

            return sum;
        }
        public void UpdateCurrencyValue()
        {
            //Uppdatera listan med random värden innom en rimlig range av dens standardvärde. 
        }

    }
}
