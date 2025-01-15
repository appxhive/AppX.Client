using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Entities.UserAccount;

namespace AppX.Client.Domain.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<ApiResponse> ConfirmEmailAsync(string userId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(UserProfile user);
        Task<bool> SendEmailConfirmationTokenAsync(UserProfile user, string recipient);
        Task<ApiResponse> LogInUserAsync(LoginUserModel loginUserDto);
    }
}
