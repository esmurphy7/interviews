## User Roles
- https://www.interviewdb.io/question/stripe?page=2&name=user-roles 

You are given two inputs:

`accounts`: a list of account objects, each with:
- `accountId`: the unique identifier of the account.
- `parent`: the accountId of its parent account or None if it's a root.

`user_role_assignments`: a list of user role assignment objects, each with:
- `userId`: the unique identifier of the user.
- `accountId`: the account to which the role applies.
- `role`: the role assigned to the user on the given account.
Implement the following functionalities:

Example:

```json
accounts = [{"accountId": "org_1", "parent": None}, {"accountId": "wksp_1", "parent": "org_1"}, ...]

user_role_assignments = [{"userId": "usr_1", "accountId" : "org_1", "role" : "admin"}, ...]
```

### Part 1
Write a function `getRoles(userId: str, accountId: str) -> List[str]` that returns all roles assigned to the user for the given account.

### Part 2
Extend `getRoles` to also include roles from the account's parent and grandparent accounts (if any). The account hierarchy is a tree with no cycles and at most two levels above any node (i.e., at most grandparent → parent → child). Return all roles assigned to the user on the given account and its ancestors.

### Part 3
Write a function `getUsersForAccount(accountId: str) -> List[str]` that returns users across the account and its ancestors.

### Part 4
Extend `getUsersForAccount` with an additional argument `roleFilters: List[str]`. Write a function `getUsersForAccountWithFilter(accountId: str, roleFilters: List[str]) -> List[str]` that returns users who have all roles in roleFilters across the account and its ancestors. A user must have each role in the filter set somewhere in the account’s ancestry (including the account itself) to be included in the output.