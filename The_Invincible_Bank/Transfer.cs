using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace The_Invincible_Bank
{
    internal class TransferMoney
    {
        private BankAccount accountSender;
        private BankAccount accountReceiver;
        private decimal balance;
        private decimal amount;
        private DateTime transactionTime;

        public TransferMoney(BankAccount accountSender, BankAccount accountReceiver, decimal amount, DateTime transactionTime)
        {
            this.accountSender = accountSender;
            this.accountReceiver = accountReceiver;
            this.amount = amount;
            this.transactionTime = transactionTime;
            this.balance = 0m;
        }

        public bool ExecuteTransfer()
        {
            // Validate that both accounts exist
            if (accountSender == null || accountReceiver)
            {
                System.Console.WriteLine("Error: One of both accounts are null.");
                return false;
            }
            
            // Validate that amount is positive
        }
           
    }

    internal class Transaction
    {
        public DateTime Time { get; set; }
        public string Type { get; set; } // Internal or External
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Description { get; set; }
    }
}