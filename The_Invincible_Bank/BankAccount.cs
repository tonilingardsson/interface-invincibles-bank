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

        // Toni: jag hämtar currency från WorldMarket och använder int till AccountNumber
        // Gammal -> public string CurrencyType { get; private set; }
        public WorldMarket.Currency CurrencyType { get; private set; }
        // Gammal -> public string AccountNumber { get; private set; }
        public int AccountNumber { get; private set; }
        // Lägger till en ny egenskap till räntan
        public decimal InterestRate { get; private set; }

        private string filePath;


        public BankAccount()
        {

        }
        public BankAccount(string name, string currencyType, string accountNumber)
        {
            Name = name;
            Sum = 0m;
            CurrencyType = currencyType;
            AccountNumber = accountNumber;

            string fileName = $"account_{accountNumber}.txt";

            string projectPath = Directory.GetCurrentDirectory();
            filePath = Path.Combine(projectPath, fileName);

            CreateAccountFile();
        }
        private void CreateAccountFile()
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine($"Account Number: {AccountNumber}");
                sw.WriteLine($"Account Name: {Name}");
                sw.WriteLine($"Currency Type: {CurrencyType}");
                sw.WriteLine($"Balance: {Sum} {CurrencyType}");
                sw.WriteLine($"--- Transactions history --- ");
            }
            UI.DisplayMessage($"Account file Created: {filePath}");
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Sum += amount;
                UI.DisplayMessage($"Deposited {amount} {CurrencyType}. New balance: {Sum} {CurrencyType}");
            }
        }
        public void Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= Sum)
            {
                Sum -= amount;
                UI.DisplayMessage($"Withdrew {amount} {CurrencyType}. New balance: {Sum} {CurrencyType}");
            }
            else
            {
                UI.DisplayMessage("Insufficient funds or invalid amount!");
            }
        }

        public void WriteToFile(string transactionInfo)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"{DateTime.Now}: {transactionInfo}");
            }

            // Sätter räntan till 1% i constructor
            InterestRate = 0.01m;

        }
    }
}
