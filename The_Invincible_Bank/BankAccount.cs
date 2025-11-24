using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.IO;
using System.Threading.Tasks;


namespace The_Invincible_Bank
{
    internal class BankAccount
    {
        public string Name {  get; private set; } = string.Empty;
        public decimal Sum { get; private set; } = 0;
        public WorldMarket.Currency CurrencyType { get; private set; }
        // public int AccountNumber { get; private set; } Provar att arvända
        public string AccountNumber { get; set; } // assus exists or add one
        public decimal Balance { get; private set; } // make sure Balance can be read

        public BankAccount()
        {
            
        }
        public BankAccount (string name, WorldMarket.Currency currencyType, string accountNumber)
        {
            Name = name;
            // Sum = 0m;
            Balance = 0m;
            CurrencyType = currencyType;
            AccountNumber = accountNumber;
        }

        // Transactions history for this account
        private readonly List<Transaction> transactions = new List<Transaction>();

        // Expose a read-only copy
        public IReadOnlyList<Transaction> Transactions => transactions.AsReadOnly();

        // Currency Trader (for now KISS -> Deposit) helper: updates balance and records a transaction
        public void Deposit(decimal amount, DateTime time, string description = null)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be positive", nameof(account));
            if (Balance < amount) throw new InvalidOperationException("Insufficient funds");
            Balance += amount;
            transactions.Add(new Transaction
            {
                Time = time;
                Type = "Internal",
                Amount = amount,
                BalanceAfter = Balance,
                Description = description ?? "Deposit/Internal"
            });
        }

        // External Tx (for now KISS -> Withdraw) helper: updates balance and records a transaction to an external account
        public void Withdraw(decimal amount, DateTime time, string description = null)
        {
            if (amount <= 0) throw new ArgumentException("Transaction amount must be positive", nameof(amount));
            if (Balance < amount) throw new InvalidOperationException("Insufficient funds");
            BalanceSender -= amount;
            transactions.Add(new Transaction
            {
                Time = time,
                type = "Debit",
                Amount = amount,
                BalanceAfter = Balance,
                Description = description ?? "Withdrawal/External"
            });
        }

        // Save this account's transactions to a JSON file
        public void SaveTransactionsToFile(string path)
        {
            var json = JsonSerializer.Serialize(transaction, new JsonSerializerOptions { WriteIndented = true});
            File.WriteAllText(path, json);

            // Load transactions from file (replace current history)
            public void LoadTransactionsFromFile(string path)
            {
                if (!File.Exists(path)) return;
                var json = File.ReadAllText(path);
                var loaded = JsonSerializer.Deserialize<List<Transaction>>(json);
                if(loaded !=null)
                {
                    transactions.Clear();
                    transactions.AddRange(loaded);
                    // Optionally update Balance to last item's BalanceAfter
                    if (transactions.Count > 0)
                    {
                        Balance = transactions[^1].BalanceAfter;
                    }
                }
            }

            // Convenience: return formatted history string
            public string GetFormattedTransactionHistory()
            {
                var sb = new System.Text.StringBuilder();
                foreach (var tx in transactions)
                {
                    sb.AppendLine($"{tx.Time:yyyy-MM-dd HH:mm:ss} | {tx.Type, -6} | {tx.Amount, 10:C} | {tx.BalanceAfter} | {tx.Description}");
                }
                return sb.ToString();
            }
        }
    }
}
