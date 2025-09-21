public class Program
{
    public static void Main(string[] args)
    {
        var solution = new Solution();

        // =====================
        // Part 1
        // =====================
        // solution.CreateAccount(1, "account1");
        // solution.CreateAccount(2, "account1");
        // solution.CreateAccount(3, "account2");
        // solution.PrintAll();

        // solution.Deposit(4, "non-existing", 2700);
        // solution.PrintAll();

        // solution.Deposit(5, "account1", 2700);
        // solution.PrintAll();

        // solution.Transfer(6, "account1", "account2", 200);
        // solution.PrintAll();

        // =====================
        // Part 2
        // // =====================
        // solution.CreateAccount(1, "account3");
        // solution.CreateAccount(2, "account2");
        // solution.CreateAccount(3, "account1");
        // solution.Deposit(4, "account1", 2000);
        // solution.Deposit(5, "account2", 3000);
        // solution.Deposit(6, "account3", 4000);

        // var topSpenders = solution.GetTopSpenders(7, 3);
        // Console.WriteLine($"topSpenders: {string.Join(',', topSpenders)}");

        // solution.Transfer(8, "account3", "account2", 500);
        // solution.Transfer(9, "account3", "account1", 1000);
        // solution.Transfer(10, "account1", "account2", 2500);
        // solution.PrintAll();

        // topSpenders = solution.GetTopSpenders(11, 3);
        // Console.WriteLine($"topSpenders: {string.Join(',', topSpenders)}");

        // =====================
        // Part 3
        // =====================
        const int MsPerDay = 86400000;
        solution.CreateAccount(1, "account1");
        solution.CreateAccount(2, "account2");
        solution.Deposit(3, "account1", 2000);
        var payment = solution.Pay(4, "account1", 1000);
        Console.WriteLine($"payment: {payment}");

        payment = solution.Pay(100, "account1", 1000);
        Console.WriteLine($"payment: {payment}");

        var status = solution.GetPaymentStatus(101, "non-existing", "payment1");
        Console.WriteLine($"payment1 status: {status}");

        status = solution.GetPaymentStatus(102, "account2", "payment1");
        Console.WriteLine($"payment1 status: {status}");

        status = solution.GetPaymentStatus(103, "account1", "payment1");
        Console.WriteLine($"payment1 status: {status}");

        var topSpenders = solution.GetTopSpenders(104, 2);
        Console.WriteLine($"topSpenders: {string.Join(',', topSpenders)}");

        var balance = solution.Deposit(3 + MsPerDay, "account1", 100);
        Console.WriteLine($"account1 balance: {balance}");

        status = solution.GetPaymentStatus(4 + MsPerDay, "account1", "payment1");
        Console.WriteLine($"payment1 status: {status}");

        balance = solution.Deposit(5 + MsPerDay, "account1", 100);
        Console.WriteLine($"account1 balance: {balance}");        

        balance = solution.Deposit(99 + MsPerDay, "account1", 100);
        Console.WriteLine($"account1 balance: {balance}");        

        balance = solution.Deposit(100 + MsPerDay, "account1", 100);
        Console.WriteLine($"account1 balance: {balance}");        
    }
}