using AppX.Client.Domain.Entities.AppUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.AppUsers.Commands.RegisterUser
{
    public class RegisterUserCommandHandler(
        ILogger<RegisterUserCommandHandler> logger,
        IUserStore<UserProfile> userStore
        ) : IRequestHandler<RegisterUserCommand>
    {
        public Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
