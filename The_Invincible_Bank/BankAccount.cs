using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    public class BankAccount
    {
        public string Name { get; private set; } = string.Empty;
        public decimal Sum { get; private set; } = 10;
        public string CurrencyType { get; private set; }
        public string AccountNumber { get; private set; }

        public string FilePath { get; private set; }

        public BankAccount()
        {

        }
        public BankAccount(string name, string currencyType, string accountNumber)
        {
            Name = name;
            Sum = Currency.Convert("SEK", currencyType, 10);
            CurrencyType = currencyType;
            AccountNumber = accountNumber;
            //Create filename based on account number
            string fileName = $"account_{accountNumber}.txt";

            string projectPath = Directory.GetCurrentDirectory();
            FilePath = Path.Combine(projectPath, fileName);

            CreateAccountFile();
        }
        private void CreateAccountFile()
        {
            using (StreamWriter sw = File.CreateText(FilePath))
            {
                sw.WriteLine($"Account Number: {AccountNumber}");
                sw.WriteLine($"Account Name: {Name}");
                sw.WriteLine($"Currency Type: {CurrencyType}");
                sw.WriteLine($"Balance: {Sum} {CurrencyType}");
                sw.WriteLine($"--- Transactions history --- ");
            }
           
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Sum += amount;
                WriteToFile($"Deposited {amount} {CurrencyType}. New balance: {Sum} {CurrencyType}");
                //UI.DisplayMessage($"Deposited {amount} {CurrencyType}. New balance: {Sum} {CurrencyType}");
            }
        }
        public void Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= Sum)
            {
                Sum -= amount;
                WriteToFile($"Withdrew {amount} {CurrencyType}. New balance: {Sum} {CurrencyType}");
                //UI.DisplayMessage($"Withdrew {amount} {CurrencyType}. New balance: {Sum} {CurrencyType}");
            }
            else
            {
                //UI.DisplayMessage("Insufficient funds or invalid amount!");
            }
        }

        public void WriteToFile(string transactionInfo)
        {
            using (StreamWriter sw = File.AppendText(FilePath))
            {
                sw.WriteLine($"{DateTime.Now}: {transactionInfo}");
            }
        }
    }
}
