using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Interfaces.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.RegisterUser
{
    public class RegisterUserCommandHandler
               (ILogger<RegisterUserCommandHandler> logger,
               UserManager<UserProfile> userManager,
               IIdentityService identityService)
               : IRequestHandler<RegisterUserCommand, bool>
        {
            public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Registering a new user {@User}", request.Email);

                //apply auto mapping - when payload is correct, all required props are supplied
                var user = new UserProfile
                {
                    UserName = request.Email, //Temporarily make email as user name
                    Email = request.Email,
                    PasswordHash = request.Password, //implement Password salt and password encryption or password hash
                };

                var result = await userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    return await identityService.SendEmailConfirmationTokenAsync(user, request.Email);
                }

                return false;
            }
        }
 }
