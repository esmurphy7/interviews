public class Program
{
    public static void Main()
    {
        var solution = new Solution();

        // ================
        // Part 1
        // ================
        // solution.Set("A", "B", "E");
        // solution.PrintState();

        // solution.Set("A", "C", "F");
        // solution.PrintState();

        // var result = solution.Get("A", "B");
        // Console.WriteLine($"get A.B: {result}");

        // result = solution.Get("A", "D");
        // Console.WriteLine($"get A.D: {result}");

        // result = solution.Delete("A", "B");
        // Console.WriteLine($"delete A.B: {result}");
        // solution.PrintState();

        // result = solution.Delete("A", "D");
        // Console.WriteLine($"delete A.D: {result}");
        // solution.PrintState();

        // ================
        // Part 2
        // ================
        solution.Set("A", "BC", "E");
        solution.Set("A", "BD", "F");
        solution.Set("A", "C", "G");
        solution.PrintState();

        var result = solution.ScanByPrefix("A", "B");
        Console.WriteLine($"scanbyprefix A.B: {result}");

        result = solution.Scan("A");
        Console.WriteLine($"scan A: {result}");

        result = solution.ScanByPrefix("B", "B");
        Console.WriteLine($"scanbyprefix B.B: {result}");
    }
}