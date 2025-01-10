namespace AppX.Client.Application.BusinessCore.UserAccounts
{
    public record CurrentUserAccount(string Id,
        string Email,
        IEnumerable<string> Roles,
        string? Nationality,
        DateOnly? DateOfBirth)
    {
        public bool IsInRole(string role) => Roles.Contains(role);
    }
}
