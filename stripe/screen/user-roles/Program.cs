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

        var roleAssignments = new List<string>
        {
            "{ \"userId\": \"user1\", \"accountId\" : \"org1\", \"role\" : \"admin\"}",
        };

        var solution = new Solution(accounts, roleAssignments);
        var result = solution.GetRoles("user1", "org1");
        Console.WriteLine($"GetRoles user1, org1: {string.Join(",", result)}");

        // ===================
        // Part 2
        // ===================
        accounts = new List<string>
        {
            "{\"accountId\": \"org1\", \"parent\": \"null\"}",
            "{\"accountId\": \"workspace1\", \"parent\": \"org1\"}",
            "{\"accountId\": \"project1\", \"parent\": \"workspace1\"}",
            "{\"accountId\": \"org2\", \"parent\": \"null\"}",
        };

        roleAssignments = new List<string>
        {
            "{ \"userId\": \"user1\", \"accountId\" : \"org1\", \"role\" : \"reader\"}",
            "{ \"userId\": \"user1\", \"accountId\" : \"workspace1\", \"role\" : \"writer\"}",
            "{ \"userId\": \"user1\", \"accountId\" : \"project1\", \"role\" : \"admin\"}",
            "{ \"userId\": \"user1\", \"accountId\" : \"org2\", \"role\" : \"reader\"}",
        };

        solution = new Solution(accounts, roleAssignments);
        result = solution.GetInheritedRoles("user1", "org1");
        Console.WriteLine($"GetInheritedRoles user1,org1: {string.Join(",", result)}");
        result = solution.GetInheritedRoles("user1", "workspace1");
        Console.WriteLine($"GetInheritedRoles user1,workspace1: {string.Join(",", result)}");
        result = solution.GetInheritedRoles("user1", "project1");
        Console.WriteLine($"GetInheritedRoles user1,project1: {string.Join(",", result)}");

        result = solution.GetInheritedRoles("user1", "org2");
        Console.WriteLine($"GetInheritedRoles user1,org2: {string.Join(",", result)}");
    }
}