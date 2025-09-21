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
        // =====================
        solution.CreateAccount(1, "account3");
        solution.CreateAccount(2, "account2");
        solution.CreateAccount(3, "account1");
        solution.Deposit(4, "account1", 2000);
        solution.Deposit(5, "account2", 3000);
        solution.Deposit(6, "account3", 4000);

        var topSpenders = solution.GetTopSpenders(7, 3);
        Console.WriteLine($"topSpenders: {string.Join(',', topSpenders)}");

        solution.Transfer(8, "account3", "account2", 500);
        solution.Transfer(9, "account3", "account1", 1000);
        solution.Transfer(10, "account1", "account2", 2500);
        solution.PrintAll();

        topSpenders = solution.GetTopSpenders(11, 3);
        Console.WriteLine($"topSpenders: {string.Join(',', topSpenders)}");
    }
}