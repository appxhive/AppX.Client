namespace AppX.Client.Application.BusinessCore.UserAccounts.UserAccountContext
{
    public interface IUserAccountContext
    {
        CurrentUserAccount? GetCurrentAppUser();
    }
}