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
        public int AccountNumber { get; private set; }
        // Lägger till en ny egenskap till räntan
        public decimal InterestRate { get; private set; }

        public BankAccount()
        {

        }
        public BankAccount(string name, WorldMarket.Currency currencyType, int accountNumber)
        {
            Name = name;
            Sum = 0m;
            CurrencyType = currencyType;
            AccountNumber = accountNumber;
            // Sätter räntan till 1% i constructor
            InterestRate = 0.01m;
        }
    }
}
