using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Entities.UserAccount;

namespace AppX.Client.Domain.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<ApiResponse> ConfirmEmailAsync(string userId, string token);
        Task<bool> SendEmailConfirmationTokenAsync(UserProfile user, string recipient);
        Task<ApiResponse> LogInUserAsync(LoginUserModel loginUserDto);
        Task<ApiResponse> EnableAuthenticatorAsync();
        Task<ApiResponse> EnableAuthenticatorAsync(TwoFactorAuthentication twoFactorAuth);
        Task<ApiResponse> SignOutAsync();
        Task<ApiResponse> VerifyAuthenticatorCodeAsync(bool rememberMe = false);
        Task<ApiResponse> VerifyAuthenticatorCodeAsync(VerifyAuthenticatorDto model);
        Task<ApiResponse> RemoveAuthenticator();
        ApiResponse ForgotPassword();
        Task<ApiResponse> ForgotPassword(ForgotPasswordDto model);
    }
}
