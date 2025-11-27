using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class BankAccount
    {
        public string Name { get; private set; } = string.Empty;
        public decimal Sum { get; private set; } = 0;
        public WorldMarket.Currency CurrencyType { get; private set; }
        public string AccountNumber { get; private set; }

        public BankAccount()
        {

        }
        public BankAccount(string name, WorldMarket.Currency currencyType, string accountNumber)
        {
            Name = name;
            Sum = 0m;
            CurrencyType = currencyType;
            AccountNumber = accountNumber;
        }
    }
}
