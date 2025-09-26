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
        Console.WriteLine($"====================================");
        Console.WriteLine($"Part 1");
        Console.WriteLine($"====================================");

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
        Console.WriteLine($"====================================");
        Console.WriteLine($"Part 2");
        Console.WriteLine($"====================================");

        result = solution.GetInheritedRoles("user1", "org1");
        Console.WriteLine($"GetInheritedRoles user1,org1: {string.Join(",", result)}");
        result = solution.GetInheritedRoles("user1", "workspace1");
        Console.WriteLine($"GetInheritedRoles user1,workspace1: {string.Join(",", result)}");
        result = solution.GetInheritedRoles("user1", "project1");
        Console.WriteLine($"GetInheritedRoles user1,project1: {string.Join(",", result)}");

        result = solution.GetInheritedRoles("user1", "org2");
        Console.WriteLine($"GetInheritedRoles user1,org2: {string.Join(",", result)}");

        // ===================
        // Part 3
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
            "{ \"userId\": \"user2\", \"accountId\" : \"workspace1\", \"role\" : \"writer\"}",
            "{ \"userId\": \"user3\", \"accountId\" : \"project1\", \"role\" : \"admin\"}",
            "{ \"userId\": \"user1\", \"accountId\" : \"org2\", \"role\" : \"reader\"}",
        };

        solution = new Solution(accounts, roleAssignments);
        Console.WriteLine($"====================================");
        Console.WriteLine($"Part 3");
        Console.WriteLine($"====================================");

        result = solution.GetUsersForAccount("org1");
        Console.WriteLine($"GetUsersForAccount org1: {string.Join(",", result)}");

        result = solution.GetUsersForAccount("workspace1");
        Console.WriteLine($"GetUsersForAccount workspace1: {string.Join(",", result)}");

        result = solution.GetUsersForAccount("project1");
        Console.WriteLine($"GetUsersForAccount project1: {string.Join(",", result)}");

        result = solution.GetUsersForAccount("org2");
        Console.WriteLine($"GetUsersForAccount org2: {string.Join(",", result)}");

        // ===================
        // Part 4
        // ===================
        // Test: Get users for an account and its ancestors who have all roles in the filter set somewhere in the ancestry
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
            "{ \"userId\": \"user1\", \"accountId\" : \"org2\", \"role\" : \"reader\"}",
            "{ \"userId\": \"user2\", \"accountId\" : \"org1\", \"role\" : \"reader\"}",
            "{ \"userId\": \"user2\", \"accountId\" : \"workspace1\", \"role\" : \"writer\"}",
            "{ \"userId\": \"user2\", \"accountId\" : \"project1\", \"role\" : \"admin\"}",
            "{ \"userId\": \"user3\", \"accountId\" : \"org1\", \"role\" : \"reader\"}",
            "{ \"userId\": \"user3\", \"accountId\" : \"workspace1\", \"role\" : \"writer\"}",
            "{ \"userId\": \"user3\", \"accountId\" : \"project1\", \"role\" : \"admin\"}",
        };

        solution = new Solution(accounts, roleAssignments);
        Console.WriteLine($"====================================");
        Console.WriteLine($"Part 4");
        Console.WriteLine($"====================================");

        // Test: Users for project1 with role filter [reader]
        var roleFilters = new List<string> { "reader" };
        result = solution.GetUsersForAccountWithFilter("project1", roleFilters);
        Console.WriteLine($"GetUsersForAccountWithFilter project1, [reader]: {string.Join(",", result)}"); // Should print: user1,user2,user3

        // Test: Users for project1 with role filter [admin]
        roleFilters = new List<string> { "admin" };
        result = solution.GetUsersForAccountWithFilter("project1", roleFilters);
        Console.WriteLine($"GetUsersForAccountWithFilter project1, [admin]: {string.Join(",", result)}"); // Should print: user2,user3

        // Test: Users for project1 with role filter [reader,admin]
        roleFilters = new List<string> { "reader", "admin" };
        result = solution.GetUsersForAccountWithFilter("project1", roleFilters);
        Console.WriteLine($"GetUsersForAccountWithFilter project1, [reader,admin]: {string.Join(",", result)}"); // Should print: user2,user3

        // Test: Users for project1 with role filter [writer,admin]
        roleFilters = new List<string> { "writer", "admin" };
        result = solution.GetUsersForAccountWithFilter("project1", roleFilters);
        Console.WriteLine($"GetUsersForAccountWithFilter project1, [writer,admin]: {string.Join(",", result)}"); // Should print: user2,user3

        // Test: Users for project1 with role filter [reader,writer,admin]
        roleFilters = new List<string> { "reader", "writer", "admin" };
        result = solution.GetUsersForAccountWithFilter("project1", roleFilters);
        Console.WriteLine($"GetUsersForAccountWithFilter project1, [reader,writer,admin]: {string.Join(",", result)}"); // Should print: user3
    }
}