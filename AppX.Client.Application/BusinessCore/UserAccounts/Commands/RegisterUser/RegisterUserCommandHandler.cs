using AppX.Client.Domain.Entities.UserAccount;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.RegisterUser
{
        public class RegisterUserCommandHandler
               (ILogger<RegisterUserCommandHandler> logger,
               IMapper mapper,
               UserManager<UserProfile> userManager)
               : IRequestHandler<RegisterUserCommand, bool>
        {
            public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("{UserEmail} Registering a new user {@User}",
                  request.Email,
                  request);

                var newUser = new UserProfile
                {
                    UserName = request.Email, //Temporarily make email as Username
                    Email = request.Email,
                    PasswordHash = request.Password,
                };

                var result = await userManager.CreateAsync(newUser, request.Password);

                if (result.Succeeded)
                {
                    return true;
                }

                return false;
            }
        }
 }
