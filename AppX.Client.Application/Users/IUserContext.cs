namespace AppX.Client.Application.Users
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }
}