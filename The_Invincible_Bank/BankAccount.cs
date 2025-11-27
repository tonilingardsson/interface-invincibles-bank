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
        public string CurrencyType { get; private set; }
        public int AccountNumber { get; private set; }

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
            //Create filename based on account number
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
                sw.WriteLine($"Account Number: {AccountNumber}");
            }
            UI.DisplayMessage($"Account file Created: {filePath}");
        }
    }
}
