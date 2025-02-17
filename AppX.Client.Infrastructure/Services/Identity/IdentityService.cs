using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Interfaces.Email;
using AppX.Client.Domain.Interfaces.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AppX.Client.Infrastructure.Services.Identity
{
    internal class IdentityService(
        UserManager<UserProfile> userManager,
        IConfiguration configuration,
        IActionContextAccessor actionContextAccessor,
        IUrlHelperFactory urlHelperFactory,
        IEmailComposer emailComposer,
        SignInManager<UserProfile> signInManager,
        IHttpContextAccessor httpContextAccesor,
        UrlEncoder urlEncoder) : IIdentityService
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

        public async Task<bool> SendEmailConfirmationTokenAsync(UserProfile user, string recipient)
        {
            var url = $"{configuration["AppXHive:Host"]}" + "Email/SendAsync";

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

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
                $"Please confirm your : <a href=\"{callbackUrl}\">confirm link</a>" +
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
                    httpClient.Dispose();
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

            if (result.RequiresTwoFactor) //search how and when to enable two factor authentication or Seperate API that controls this RequiresTwoFactor flag 
            {
                response.StatusCode = HttpStatusCode.Accepted;
                response.ResponseMessage = "Two factor authentication is required.";
                //redirect to Page where VerifyAuthenticatorCode is.
                return response;
            }

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
                ArgumentNullException.ThrowIfNull(httpContextAccesor.HttpContext);
                ClaimsPrincipal currentUser = httpContextAccesor.HttpContext.User;
                var userAccount = await userManager.GetUserAsync(currentUser);
                //var te = userManager.GenerateUserTokenAsync(userAccount, );
                //var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

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

        public async Task<ApiResponse> EnableAuthenticatorAsync()
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };

            ArgumentNullException.ThrowIfNull(httpContextAccesor.HttpContext);

            ClaimsPrincipal currentUser = httpContextAccesor.HttpContext.User;

            // Access claims from the ClaimsPrincipal
            //string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //string userEmail = currentUser.FindFirst(ClaimTypes.Email)?.Value;

            var user = await userManager.GetUserAsync(currentUser);

            ArgumentNullException.ThrowIfNull(currentUser.Identity);

            if (user is not null && currentUser.Identity.IsAuthenticated)
            {
                //resets old token key previous creating new one.
                await userManager.ResetAuthenticatorKeyAsync(user);

                var token = await userManager.GetAuthenticatorKeyAsync(user);

                string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

                string authUri = string.Format(AuthenticatorUriFormat,
                urlEncoder.Encode("AppXHive"), urlEncoder.Encode(user.Email), token);

                //string secretKey = _config["TOTPSecretKey"]!;

                //string code = "";

                //if (!string.IsNullOrEmpty(token))
                //{
                //    code = TOTPGenerator.GenerateCode(token);
                //}

                var model = new TwoFactorAuthentication()
                {
                    /*Code = code, */ //does not need to pass here. only used in Post Auth. the code from mobile app to AppXHive app
                    Token = token, //from AppXHive to mobile app - to setup mobile app auth
                    QRCodeUrl = authUri
                };

                //use either the token or QRCodeUrl and use it to your Authenticator App on your Mobile.
                response.StatusCode = HttpStatusCode.OK;
                response.ResponseMessage = "Enables Two Factor Authentication.";
                response.Data = model;
                response.Success = true;
                return response;
            }

            //Account = AppXHive
            //Secret = Token or QRCode or Code
            response.ResponseMessage = "User is either does not exists or not signed in.";

            return response;
        }

        public async Task<ApiResponse> EnableAuthenticatorAsync(TwoFactorAuthentication twoFactorAuth)
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };

            ArgumentNullException.ThrowIfNull(httpContextAccesor.HttpContext);

            ClaimsPrincipal currentUser = httpContextAccesor.HttpContext.User;

            var user = await userManager.GetUserAsync(currentUser);

            if (user is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Data = user;
                response.ResponseMessage = "User not found or does not exists.";
                return response;
            }

            ArgumentNullException.ThrowIfNull(currentUser.Identity);

            if (user is not null 
                && currentUser.Identity.IsAuthenticated 
                && twoFactorAuth.Code is not null)
            {
                response.Data = user;

                var succeeded = await userManager.VerifyTwoFactorTokenAsync(user,
                        userManager.Options.Tokens.AuthenticatorTokenProvider, twoFactorAuth.Code);

                if (succeeded)
                {
                    await userManager.SetTwoFactorEnabledAsync(user, true);
                    response.StatusCode = HttpStatusCode.OK;
                    response.ResponseMessage = "Your two factor auth has been enabled.";
                    response.Success = true;
                    return response;
                }
                else
                {
                    response.ResponseMessage = "Your two factor auth code could not be validated.";

                    return response;
                }
            }

            return response;
        }

        public async Task<ApiResponse> SignOutAsync()
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };

            ArgumentNullException.ThrowIfNull(httpContextAccesor.HttpContext);

            await signInManager.SignOutAsync();
            await httpContextAccesor.HttpContext.SignOutAsync();

            ClaimsPrincipal currentUser = httpContextAccesor.HttpContext.User;

            //var user = await userManager.GetUserAsync(currentUser);

            //ArgumentNullException.ThrowIfNull(user);

            //IdentityResult result = await userManager.SetTwoFactorEnabledAsync(user, false);

            //if (result.Succeeded)
            //{
            //    await signInManager.SignOutAsync();
            //    await httpContextAccesor.HttpContext.SignOutAsync();
            //}

            var IsSignedIn = signInManager.IsSignedIn(currentUser);

            if (!IsSignedIn)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Data = IsSignedIn;
                response.ResponseMessage = "Successfully signed out.";
            }

            return response;
        }

        public async Task<ApiResponse> VerifyAuthenticatorCodeAsync(bool rememberMe = false)
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if(user == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ResponseMessage = "Invalid or user does not exist.";
                
                return response;
            }

            response.ResponseMessage = "Please verify authentication code.";

            var verifyAuthenticatorDto = new VerifyAuthenticatorDto
            {
                RememberMe = rememberMe
            };

            response.Data = verifyAuthenticatorDto;

            return response;
        }

        public async Task<ApiResponse> VerifyAuthenticatorCodeAsync(VerifyAuthenticatorDto model)
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null
            };   

            var actionContext = actionContextAccessor.ActionContext!;

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);

            model.ReturnUrl = model.ReturnUrl ?? urlHelper.Content("~/");

            var result = await signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RememberMe, rememberClient: false);

            response.Data = result;

            if(result.Succeeded)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.ResponseMessage = "Authentication succeeded.";
                return response;
            }

            return response;
        }

        public async Task<ApiResponse> RemoveAuthenticator()
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };

            ArgumentNullException.ThrowIfNull(httpContextAccesor.HttpContext);

            ClaimsPrincipal currentUser = httpContextAccesor.HttpContext.User;

            var user = await userManager.GetUserAsync(currentUser);

            response.Data = user;

            if (user is not null)
            {
                await userManager.ResetAuthenticatorKeyAsync(user);

                await userManager.SetTwoFactorEnabledAsync(user, false);

                response.StatusCode = HttpStatusCode.OK;

                response.ResponseMessage = "Two Factor Authentication has been reset.";
            }

            return response;
        }
        public ApiResponse ForgotPassword()
        {
            return new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };
        }

        public async Task<ApiResponse> ForgotPassword(ForgotPasswordDto model)
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                MetaData = null,
                ResponseMessage = null,
            };

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ResponseMessage = "User not found or does not exits!";
                return response;
            }

            var url = $"{configuration["AppXHive:Host"]}" + "Email/SendAsync";

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var actionContext = actionContextAccessor.ActionContext!;

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);

            var callbackUrl = urlHelper.Action("ResetPassword", "Account", new
            {
                userid = user.Id,
                code = token
            }, protocol: actionContext.HttpContext.Request.Scheme);

            var composedEmail = emailComposer.Compose(
                "noreply@appxhive.com",
                $"{model.Email}",
                "",
                "Reset password email confirmation",
                $"Please confirm your : <a href=\"{callbackUrl}\">confirm link</a>" +
                "<br/>" +
                "<p>Powered By: <p/>" +
                "<h1>AppXHive Software Development Services<h1/>",
                "",
                $"{configuration["AppXClient:RegisterUser:ResetConfirmation1"]}"
            );

            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonSerializer.Serialize(composedEmail);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var result = await httpClient.PostAsync(url, content);
                result.EnsureSuccessStatusCode();
                await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    httpClient.Dispose();
                    response.StatusCode = HttpStatusCode.OK;
                    response.ResponseMessage = "A reset confirmation email has been sent to your email.";

                    return response;
                }
            }

            response.ResponseMessage = "Unable to send reset confirmation email to your email. " +
                                       "Please confirm you have entered correct and valid email address.";

            return response;
        }

    }
}
