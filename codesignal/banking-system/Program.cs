public class Program
{
    public static void Main(string[] args)
    {
        var solution = new Solution();

        // =====================
        // Part 1
        // =====================
        solution.CreateAccount(1, "account1");
        solution.CreateAccount(2, "account1");
        solution.CreateAccount(3, "account2");
        solution.PrintAll();

        solution.Deposit(4, "non-existing", 2700);
        solution.PrintAll();

        solution.Deposit(5, "account1", 2700);
        solution.PrintAll();

        solution.Transfer(6, "account1", "account2", 200);
        solution.PrintAll();        
    }
}