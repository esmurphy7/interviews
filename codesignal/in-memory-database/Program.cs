using System.Xml.XPath;

public class Program
{
    public static void Main()
    {
        var solution = new Solution();

        // ================
        // Level1
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
        // Level2
        // ================
        // solution.Set("A", "BC", "E");
        // solution.Set("A", "BD", "F");
        // solution.Set("A", "C", "G");
        // solution.PrintState();

        // var result = solution.ScanByPrefix("A", "B");
        // Console.WriteLine($"scanbyprefix A.B: {result}");

        // result = solution.Scan("A");
        // Console.WriteLine($"scan A: {result}");

        // result = solution.ScanByPrefix("B", "B");
        // Console.WriteLine($"scanbyprefix B.B: {result}");

        // ================
        // Level 3.1
        // ================
        // solution.SetAtWithTtl("A", "BC", "E", 1, 9);
        // solution.SetAtWithTtl("A", "BC", "E", 5, 10);
        // solution.SetAt("A", "BD", "F", 5);
        // solution.PrintState();

        // var result = solution.ScanAtPrefix("A", "B", 14);
        // Console.WriteLine($"[14]ScanAtPrefix A.B: {result}");

        // result = solution.ScanAtPrefix("A", "B", 15);
        // Console.WriteLine($"[15]ScanAtPrefix A.B: {result}");

        // ================
        // Level 3.2
        // ================
        solution.SetAt("A", "B", "C", 1);
        solution.SetAtWithTtl("X", "Y", "Z", 2, 15);
        solution.PrintState();

        var result = solution.GetAt("X", "Y", 3);
        Console.WriteLine($"[3]GetAt X.Y: {result}");

        solution.SetAtWithTtl("A", "D", "E", 4, 10);
        solution.PrintState();

        result = solution.ScanAt("A", 13);
        Console.WriteLine($"[13]ScanAt A: {result}");

        result = solution.ScanAt("X", 16);
        Console.WriteLine($"[16]ScanAt X: {result}");

        result = solution.ScanAt("X", 17);
        Console.WriteLine($"[17]ScanAt X: {result}");

        result = solution.DeleteAt("X", "Y", 20);
        Console.WriteLine($"[20]DeletAt X.Y: {result}");
    }
}