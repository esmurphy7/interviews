using System.Text.Json;
using System.Text.Json.Serialization;

public class Solution
{
    public Dictionary<string, Account> accountsById = new Dictionary<string, Account>();
    public Dictionary<string, List<RoleAssignment>> rolesByUserId = new Dictionary<string, List<RoleAssignment>>();

    public Solution(List<string> accounts, List<string> roleAssignments)
    {
        foreach (var accountStr in accounts)
        {
            var account = JsonSerializer.Deserialize<Account>(accountStr);
            this.accountsById[account.accountId] = account;
        }

        foreach (var roleStr in roleAssignments)
        {
            var roleAssignment = JsonSerializer.Deserialize<RoleAssignment>(roleStr);
            if (!this.rolesByUserId.ContainsKey(roleAssignment.userId))
            {
                this.rolesByUserId[roleAssignment.userId] = new List<RoleAssignment>();
            }

            this.rolesByUserId[roleAssignment.userId].Add(roleAssignment);
        }
    }

    public List<string> GetRoles(string userId, string accountId)
    {
        var roleStrs = new List<string>();
        if (this.rolesByUserId.TryGetValue(userId, out var roleAssignments))
        {
            roleStrs = roleAssignments.Where(r => r.accountId == accountId).Select(r => r.role).ToList();
        }

        return roleStrs;
    }

    public List<string> GetInheritedRoles(string userId, string accountId)
    {
        var roles = new List<RoleAssignment>();
        if (this.rolesByUserId.TryGetValue(userId, out var roleAssignments))
        {
            var directRoles = roleAssignments.Where(r => r.accountId == accountId).ToList();
            roles.AddRange(directRoles);

            if (this.accountsById.TryGetValue(accountId, out var directAccount))
            {
                if (directAccount.parent != null)
                {
                    var parentRoles = roleAssignments.Where(r => r.accountId == directAccount.parent).ToList();
                    roles.AddRange(parentRoles);

                    if (this.accountsById.TryGetValue(directAccount.parent, out var parentAccount))
                    {
                        if (parentAccount.parent != null)
                        {
                            var grandParentRoles = roleAssignments.Where(r => r.accountId == parentAccount.parent).ToList();
                            roles.AddRange(grandParentRoles);
                        }
                    }
                }
            }
        }

        return roles.Select(r => r.role).ToList();
    }
}

public class Account
{
    public string accountId { get; set; }
    public string? parent { get; set; }
}

public class RoleAssignment
{
    public string userId { get; set; }
    public string accountId { get; set; }
    public string role { get; set; }
}
