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
        IEmailComposer emailComposer) : IIdentityService
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
    }
}
