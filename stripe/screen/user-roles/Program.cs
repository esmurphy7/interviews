public class Program
{
    public static void Main(string[] args)
    {
        // ===================
        // Part 1
        // ===================
        var accounts = new List<string>
        {
            "{\"accountId\": \"org1\", \"parent\": \"null\"}",
            "{\"accountId\": \"workspace1\", \"parent\": \"org1\"}",
        };

        var roleAssignment = new List<string>
        {
            "{ \"userId\": \"user1\", \"accountId\" : \"org1\", \"role\" : \"admin\"}",
        };

        var solution = new Solution(accounts, roleAssignment);
        var result = solution.GetRoles("user1", "org1");
        Console.WriteLine($"GetRoles user1, org1: {string.Join(",", result)}");
    }
}