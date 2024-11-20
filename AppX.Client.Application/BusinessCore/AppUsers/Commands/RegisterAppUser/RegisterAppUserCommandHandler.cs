using AppX.Client.Domain.Entities.AppUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.AppUsers.Commands.RegisterAppUser
{
    public class RegisterAppUserCommandHandler(
        ILogger<RegisterAppUserCommandHandler> logger,
        IUserStore<UserProfile> userStore
        ) : IRequestHandler<RegisterAppUserCommand>
    {
        public Task Handle(RegisterAppUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
