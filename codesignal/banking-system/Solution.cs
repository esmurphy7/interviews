
using System.ComponentModel.DataAnnotations;
using System.Net;

public class Solution
{
    public Dictionary<string, Account> AccountsbyId { get; set; } = new Dictionary<string, Account>();
    public Dictionary<string, List<Transaction>> TxsBySrcAccountId { get; set; } = new Dictionary<string, List<Transaction>>();
    public Dictionary<string, List<Payment>> PaymentsByAccountId { get; set; } = new Dictionary<string, List<Payment>>();
    public Dictionary<string, List<Payment>> PendingPaymentsByAccountId { get; set; } = new Dictionary<string, List<Payment>>();

    public bool CreateAccount(int timestamp, string accountId)
    {
        if (this.AccountsbyId.TryGetValue(accountId, out var _))
        {
            return false;
        }

        this.AccountsbyId[accountId] = new Account
        {
            Id = accountId,
            Balance = 0,
        };

        this.TxsBySrcAccountId[accountId] = new List<Transaction>();

        return true;
    }

    public int? Deposit(int timestamp, string accountId, int amount)
    {
        if (this.AccountsbyId.TryGetValue(accountId, out var account))
        {
            this.ProcessPendingPayments(timestamp, accountId);

            account.Balance += amount;

            this.TxsBySrcAccountId[accountId].Add(new Transaction
            {
                Timestamp = timestamp,
                Amount = amount,
                SrcAccount = accountId,
                DestAccount = accountId,
                Type = TransactionType.Deposit,
            });

            return account.Balance;
        }
        else
        {
            return null;
        }
    }

    public int? Transfer(int timestamp, string srcAccountId, string destAccountId, int amount)
    {
        if (srcAccountId == destAccountId)
        {
            return null;
        }

        if (this.AccountsbyId.TryGetValue(srcAccountId, out var srcAccount))
        {
            if ((srcAccount.Balance - amount) < 0)
            {
                return null;
            }

            if (this.AccountsbyId.TryGetValue(destAccountId, out var destAccount))
            {
                this.ProcessPendingPayments(timestamp, srcAccountId);
                this.ProcessPendingPayments(timestamp, destAccountId);

                srcAccount.Balance -= amount;
                destAccount.Balance += amount;

                this.TxsBySrcAccountId[srcAccountId].Add(new Transaction
                {
                    Timestamp = timestamp,
                    Amount = amount,
                    SrcAccount = srcAccountId,
                    DestAccount = destAccountId,
                    Type = TransactionType.Transfer,
                });

                return srcAccount.Balance;
            }
        }

        return null;
    }

    public List<string> GetTopSpenders(int timestamp, int n)
    {
        var spendsByAccount = new Dictionary<string, int>();
        foreach (var accountId in this.AccountsbyId.Keys)
        {
            spendsByAccount[accountId] = 0;

            foreach (var tx in this.TxsBySrcAccountId[accountId])
            {
                if (tx.Type == TransactionType.Transfer
                    || tx.Type == TransactionType.Withdrawal)
                {
                    spendsByAccount[accountId] += tx.Amount;
                }
            }
        }

        var topSpenders = spendsByAccount.OrderByDescending(s => s.Value).ThenBy(s => s.Key).Take(n).ToDictionary();

        var result = new List<string>();
        foreach (var topSpender in topSpenders)
        {
            result.Add($"{topSpender.Key}({topSpender.Value})");
        }

        return result;
    }

    public string? Pay(int timestamp, string accountId, int amount)
    {
        if (this.AccountsbyId.TryGetValue(accountId, out var account))
        {
            if (account.Balance - amount < 0)
            {
                return null;
            }

            account.Balance -= amount;

            var payment = new Payment
            {
                Id = $"payment{this.PaymentsByAccountId.Values.Count + 1}",
                Timestamp = timestamp,
                AccountId = accountId,
                Amount = (int)(amount * 0.02),
            };

            if (!this.PaymentsByAccountId.ContainsKey(accountId))
            {
                this.PaymentsByAccountId[accountId] = new List<Payment>();
            }
            this.PaymentsByAccountId[accountId].Add(payment);

            if (!this.PendingPaymentsByAccountId.ContainsKey(accountId))
            {
                this.PendingPaymentsByAccountId[accountId] = new List<Payment>();
            }
            this.PendingPaymentsByAccountId[accountId].Add(payment);

            if (!this.TxsBySrcAccountId.ContainsKey(accountId))
            {
                this.TxsBySrcAccountId[accountId] = new List<Transaction>();
            }
            this.TxsBySrcAccountId[accountId].Add(new Transaction
            {
                Timestamp = timestamp,
                SrcAccount = accountId,
                DestAccount = accountId,
                Amount = amount,
                Type = TransactionType.Withdrawal,
            });

            return payment.Id;
        }
        else
        {
            return null;
        }
    }

    public PaymentStatus? GetPaymentStatus(int timestamp, string accountId, string paymentId)
    {
        if (!this.AccountsbyId.ContainsKey(accountId))
        {
            return null;
        }

        if (this.PaymentsByAccountId.TryGetValue(accountId, out var payments))
        {

            var payment = payments.FirstOrDefault(p => p.Id == paymentId);
            if (payment == null)
            {
                return null;
            }

            var status = payment.GetStatus(timestamp);
            return status;
        }
        else
        {
            return null;
        }
    }

    private void ProcessPendingPayments(int timestamp, string accountId)
    {
        if (this.AccountsbyId.TryGetValue(accountId, out var account))
        {
            if (this.PendingPaymentsByAccountId.TryGetValue(accountId, out var payments))
            {
                var inProgressPayments = new List<Payment>();
                foreach (var payment in payments)
                {
                    if (payment.GetStatus(timestamp) == PaymentStatus.InProgress)
                    {
                        inProgressPayments.Add(payment);
                        continue;
                    }

                    account.Balance += payment.Amount;
                }

                this.PendingPaymentsByAccountId[accountId] = inProgressPayments;
            }
        }
    }

    public void PrintAll()
    {
        Console.WriteLine($"__________");
        this.PrintAccounts();
        this.PrintTransactions();
    }

    public void PrintAccounts()
    {
        Console.WriteLine($"Accounts ({this.AccountsbyId.Keys.Count}) ======");

        foreach (var accountId in this.AccountsbyId.Keys)
        {
            this.PrintAccount(accountId);
        }
    }

    public void PrintAccount(string accountId)
    {
        if (this.AccountsbyId.TryGetValue(accountId, out var account))
        {
            Console.WriteLine($"account {account.Id} ------");
            Console.WriteLine($"balance: {account.Balance}");
        }
    }


    public void PrintTransactions()
    {
        foreach (var accountId in this.TxsBySrcAccountId.Keys)
        {
            this.PrintTransactions(accountId);
        }
    }

    public void PrintTransactions(string accountId)
    {
        if (this.AccountsbyId.TryGetValue(accountId, out var _))
        {
            Console.WriteLine($"account {accountId}'s txs ({this.TxsBySrcAccountId[accountId].Count}) ======");

            foreach (var tx in this.TxsBySrcAccountId[accountId])
            {
                Console.WriteLine($"tx at {tx.Timestamp} --------");
                Console.WriteLine($"timestamp: {tx.Timestamp}");
                Console.WriteLine($"srcAccount: {tx.SrcAccount}");
                Console.WriteLine($"destAccount: {tx.DestAccount}");
                Console.WriteLine($"typ: {tx.Type}");
            }
        }
    }
}

public class Account
{
    public string Id { get; set; }
    public int Balance { get; set; }
}

public class Transaction
{
    public int Timestamp { get; set; }
    public int Amount { get; set; }
    public string SrcAccount { get; set; }
    public string DestAccount { get; set; }
    public TransactionType Type { get; set; }
}

public enum TransactionType
{
    Deposit,
    Transfer,
    Withdrawal
}

public class Payment
{
    private const int MsPerDay = 86400000;
    public string Id { get; set; }
    public int Timestamp { get; set; }
    public string AccountId { get; set; }
    public int Amount { get; set; }

    public PaymentStatus GetStatus(int timestamp)
    {
        if (timestamp < (this.Timestamp + MsPerDay))
        {
            return PaymentStatus.InProgress;
        }

        return PaymentStatus.Received;
    }
}

public enum PaymentStatus
{
    InProgress,
    Received
}