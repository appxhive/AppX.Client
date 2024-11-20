namespace AppX.Client.Application.BusinessCore.AppUsers
{
    public record CurrentAppUser(string Id,
        string Email,
        IEnumerable<string> Roles,
        string? Nationality,
        DateOnly? DateOfBirth)
    {
        public bool IsInRole(string role) => Roles.Contains(role);
    }
}
