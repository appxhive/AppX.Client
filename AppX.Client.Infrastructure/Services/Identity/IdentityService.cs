using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Interfaces.Email;
using AppX.Client.Domain.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AppX.Client.Infrastructure.Services.Identity
{
    internal class IdentityService(UserManager<UserProfile> userManager,
        IConfiguration configuration,
        IActionContextAccessor actionContextAccessor,
        IUrlHelperFactory urlHelperFactory,
        IEmailComposer emailComposer,
        SignInManager<UserProfile> signInManager) : IIdentityService
    {
        public async Task<ApiResponse> ConfirmEmailAsync(string userId, string token)
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ResponseMessage = "User not found or does not exists!";
                return response;
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Data = result;
                response.ResponseMessage = "Email confirmed successfully!";
                response.Success = true;
                return response;
            }

            //Add token expiration

            response.ResponseMessage = "Email verification failed!";
            return response;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserProfile user)
        {
            return await userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<bool> SendEmailConfirmationTokenAsync(UserProfile user, string recipient)
        {
            var url = $"{configuration["AppXHive:Host"]}" + "Email/SendAsync";

            var token = await GenerateEmailConfirmationTokenAsync(user);

            var actionContext = actionContextAccessor.ActionContext!;

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);

            var callbackUrl = urlHelper.Action("ConfirmEmail", "UserAccounts", new
            {
                userid = user.Id,
                token
            }, protocol: actionContext.HttpContext.Request.Scheme);

            //Be careful to follow the exact sequence and properties of EmailObject from the AppXHive API
            var composedEmail = emailComposer.Compose(
                    "noreply@appxhive.com",
                    $"{recipient}",
                    "",
                    "Email confirmation",
                    $"Please confirm your email by clicking here: <a href=\"{callbackUrl}\">confirm link</a>" +
                    "<br/>" +
                    "<p>Powered By: <p/>" +
                    "<h1>AppXHive Software Development Services<h1/>",
                    "",
                    $"{configuration["AppXClient:RegisterUser:Confirmation1"]}"
                );

            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonSerializer.Serialize(composedEmail);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<ApiResponse> LogInUserAsync(LoginUserModel loginUserDto)
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };

            var user = await userManager.FindByEmailAsync(loginUserDto.Email);

            if (user == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ResponseMessage = "Incorrect email or password.";
                return response;
            }

            await userManager.GetTwoFactorEnabledAsync(user);

            var result = await signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password,
                loginUserDto.RememberMe,
                lockoutOnFailure: true);

            response.Data = result;

            if (!user.EmailConfirmed)
            {
                response.StatusCode = HttpStatusCode.Accepted;
                response.ResponseMessage = "An email verification has been sent to you. Please confirm your email."; //For additional security, require you to confirm your email first
                return response;
            }

            if (result.IsNotAllowed)
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.ResponseMessage = "User is not allowed. Please contact your administrator.";
                return response;
            }

            if (result.IsLockedOut)
            {
                response.ResponseMessage = "User is locked out.";
                return response;
            }

            //if (result.RequiresTwoFactor)
            //{
            //    response.StatusCode = HttpStatusCode.Accepted;
            //    response.ResponseMessage = "Two factor authentication is required.";
            //    //redirect to Page where VerifyAuthenticatorCode is.
            //    return response;
            //}

            //if two factor auth is enabled, redirect to Authenticator app and user must verify code before they can redirect to homepage 

            /*
                upon sign in of a user regardless of role - should send notification
                on registered mobile so, user must confirmed mobile contact.
                if user did not confirmed his/her mobile then he cannot receive notification
                from Azure AD - for two factor authentication
                TODO: setup, register app and configured on Azure AD so that notification is enable.
            */

            if (result.Succeeded)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Success = true;
                response.ResponseMessage = $"You have successfully signed in! Welcome, {loginUserDto.Email}!";
            }
            else
            {
                response.ResponseMessage = "Invalid login attempt.";
            }

            return response;
        }
    }
}
