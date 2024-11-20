using AppX.Client.Domain.Entities.AppUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.IdentityUsers.Commands.RegisterIdentityUser
{
    public class RegisterIdentityUserCommandHandler
        (ILogger<RegisterIdentityUserCommandHandler> logger,
        IMapper mapper,
        UserManager<UserProfile> userManager)
        : IRequestHandler<RegisterIdentityUserCommand, bool>
    {
        public async Task<bool> Handle(RegisterIdentityUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("{UserEmail} Registering a new user {@User}",
              request.Email,
              request);

            var newUser = new UserProfile
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = request.Password,
            };

            var result = await userManager.CreateAsync(newUser, request.PasswordHash);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
