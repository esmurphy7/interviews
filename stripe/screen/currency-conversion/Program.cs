public class Program
{
    public static void Main(string[] args)
    {
        var input = "AUD:USD:0.75,CAD:USD:0.8,CAD:MXN:13.3,MXN:AUD:0.082";
        var solution = new Solution(input);

        // ============================
        // Part 1
        // ============================

        var rate = solution.GetDirectConversionRate("AUD", "USD");
        Console.WriteLine($"AUD->USD direct rate: {rate}");

        rate = solution.GetDirectConversionRate("USD", "AUD");
        Console.WriteLine($"USD->AUD direct rate: {rate}");

        rate = solution.GetDirectConversionRate("CAD", "USD");
        Console.WriteLine($"CAD->USD direct rate: {rate}");

        rate = solution.GetDirectConversionRate("USD", "CAD");
        Console.WriteLine($"USD->CAD direct rate: {rate}");

        // ============================
        // Part 2
        // ============================

        // answer: 1.066
        rate = solution.GetSingleIntermediateConversionRate("USD", "AUD");
        Console.WriteLine($"USD->AUD rate (direct): {rate}");

        rate = solution.GetSingleIntermediateConversionRate("CAD", "AUD");
        Console.WriteLine($"CAD->AUD intermediate rate (via USD): {rate}");

        // ============================
        // Part 3
        // ============================
        rate = solution.GetBestConversionRate("USD", "MXN");
        Console.WriteLine($"USD->MXN rate (direct): {rate}");

        // answer: 1.0906
        rate = solution.GetBestConversionRate("CAD", "AUD");
        Console.WriteLine($"CAD->AUD best intermediate rate (via MXN): {rate}");

        // ============================
        // Part 4
        // ============================

        input = input + ",GBP:EUR:1.15";
        solution = new Solution(input);
        
        var possible = solution.IsConversionPossible("USD", "AUD");
        Console.WriteLine($"USD->AUD possible (direct): {possible}");
        
        possible = solution.IsConversionPossible("CAD", "AUD");
        Console.WriteLine($"CAD->AUD possible (via USD): {possible}");
        
        possible = solution.IsConversionPossible("EUR", "GBP");
        Console.WriteLine($"EUR->GBP possible (direct): {possible}");
        
        // false
        possible = solution.IsConversionPossible("CAD", "GBP");
        Console.WriteLine($"CAD->GBP possible: {possible}");
    }
}