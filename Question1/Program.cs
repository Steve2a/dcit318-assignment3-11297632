using System;
using System.Collections.Generic;

namespace Question1
{
    public readonly record struct Transaction(int Id, DateTime Date, decimal Amount, string Category);

    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }
    public sealed class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[BankTransfer] Processed {transaction.Amount:N2} for '{transaction.Category}' on {transaction.Date:yyyy-MM-dd}.");
        }
    }

    public sealed class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[MobileMoney] Sent {transaction.Amount:N2} for '{transaction.Category}' on {transaction.Date:yyyy-MM-dd}.");
        }
    }

    public sealed class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[CryptoWallet] Broadcast txn of {transaction.Amount:N2} for '{transaction.Category}' on {transaction.Date:yyyy-MM-dd}.");
        }
    }

    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number is required.", nameof(accountNumber));

            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "Initial balance cannot be negative.");

            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
            {
                Console.WriteLine("Invalid transaction: amount must be positive.");
                return;
            }
            Balance -= transaction.Amount;
            Console.WriteLine($"[Account {AccountNumber}] Deducted {transaction.Amount:N2}. New balance: {Balance:N2}");
        }
    }

    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
            {
                Console.WriteLine("Invalid transaction: amount must be positive.");
                return;
            }

            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
                return;
            }

            Balance -= transaction.Amount;
            Console.WriteLine($"[Savings {AccountNumber}] Deducted {transaction.Amount:N2}. Updated balance: {Balance:N2}");
        }
    }

    public class FinanceApp
    {
        private readonly List<Transaction> _transactions = new();

        public void Run()
        {
            
            var account = new SavingsAccount(accountNumber: "SA-1001", initialBalance: 1000m);
            Console.WriteLine($"Created SavingsAccount {account.AccountNumber} with balance {account.Balance:N2}\n");

            
            var t1 = new Transaction(Id: 1, Date: DateTime.Today, Amount: 150m, Category: "Groceries");
            var t2 = new Transaction(Id: 2, Date: DateTime.Today, Amount: 200m, Category: "Utilities");
            var t3 = new Transaction(Id: 3, Date: DateTime.Today, Amount: 75m,  Category: "Entertainment");

        
           ITransactionProcessor mobileMoney = new MobileMoneyProcessor();
            ITransactionProcessor bankTransfer = new BankTransferProcessor();
            ITransactionProcessor cryptoWallet = new CryptoWalletProcessor();

            mobileMoney.Process(t1);  
            bankTransfer.Process(t2);  
            cryptoWallet.Process(t3);  

            Console.WriteLine();

            account.ApplyTransaction(t1);
            account.ApplyTransaction(t2);
          account.ApplyTransaction(t3);

            _transactions.AddRange(new[] { t1, t2, t3 });

            Console.WriteLine("\n=== Transaction Summary ===");
            foreach (var tx in _transactions)
            {
                Console.WriteLine($"#{tx.Id}: {tx.Date:yyyy-MM-dd} | {tx.Category,-13} | {tx.Amount,8:N2}");
            }

            Console.WriteLine($"\nFinal Balance for {account.AccountNumber}: {account.Balance:N2}");
        }
    }

    public static class Program
    {        public static void Main()
        {
            var app = new FinanceApp();
            app.Run();
            Console.WriteLine("\nDone.");
        }
    }
}
