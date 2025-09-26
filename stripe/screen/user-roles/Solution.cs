using System.Reflection.Metadata;
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

    public List<string> GetUsersForAccount(string accountId)
    {
        var targetAccounts = new List<string>();
        if (this.accountsById.TryGetValue(accountId, out var directAccount))
        {
            targetAccounts.Add(directAccount.accountId);

            var childAccount = this.accountsById.Values.Where(a => a.parent == directAccount.accountId).FirstOrDefault();
            if (childAccount != null)
            {
                targetAccounts.Add(childAccount.accountId);

                var grandChildAccount = this.accountsById.Values.Where(a => a.parent == childAccount.accountId).FirstOrDefault();
                if (grandChildAccount != null)
                {
                    targetAccounts.Add(grandChildAccount.accountId);
                }
            }
        }

        var users = new List<string>();
        foreach (var roleByUserId in this.rolesByUserId)
        {
            if (roleByUserId.Value.Any(r => targetAccounts.Contains(r.accountId)))
            {
                users.Add(roleByUserId.Key);
            }
        }

        return users;
    }

    public List<string> GetUsersForAccountWithFilter(string accountId, List<string> roleFilters)
    {
        // collect all relevant accounts

        // collect all relevant user ids
        // foreach user's set of roles
        // get all roles whose account ids are in the relevant set
        // if the set of relevant roles contains all role filters
        // -> add the user to the set of relevant user ids
        // return the relevant user ids

        var relevantAccountIds = new List<string>();
        if (this.accountsById.TryGetValue(accountId, out var directAccount))
        {
            relevantAccountIds.Add(directAccount.accountId);

            if (directAccount.parent != null)
            {
                if (this.accountsById.TryGetValue(directAccount.parent, out var parentAccount))
                {
                    relevantAccountIds.Add(parentAccount.accountId);

                    if (parentAccount.parent != null)
                    {
                        if (this.accountsById.TryGetValue(parentAccount.parent, out var grandparentAccount))
                        {
                            relevantAccountIds.Add(grandparentAccount.accountId);
                        }
                    }
                }
            }
        }

        var userIds = new List<string>();
        foreach (var rolesByUserId in this.rolesByUserId)
        {
            var relevantRoles = rolesByUserId.Value.Where(r => relevantAccountIds.Contains(r.accountId)).Select(r => r.role).ToList();
            if (roleFilters.All(r => relevantRoles.Contains(r)))
            {
                userIds.Add(rolesByUserId.Key);
            }
        }

        return userIds;
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
