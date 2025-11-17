using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class BankAccount
    {
        public string Name {  get; set; }
        public decimal Sum { get; set; }
        public WorldMarket.Currency CurrencyType { get; set; }
        public int AccountNumber { get; set; }

        public BankAccount (string name, WorldMarket.Currency currencyType, int accountNumber)
        {
            Name = name;
            Sum = 0m;
            CurrencyType = currencyType;
            AccountNumber = accountNumber;
        }
    }
}
