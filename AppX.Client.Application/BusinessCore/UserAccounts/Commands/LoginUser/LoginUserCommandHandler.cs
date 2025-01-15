using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Interfaces.Identity;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.LoginUser
{
    public class LoginUserCommandHandler(
        ILogger<LoginUserCommandHandler> logger,
        IIdentityService identityService,
        IMapper mapper
        ) : IRequestHandler<LoginUserCommand, ApiResponse>
    {
        //Check user's Email has been confirmed
        public async Task<ApiResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Logging in user {request.Email}");

            var user = mapper.Map<LoginUserModel>(request);

            return await identityService.LogInUserAsync(user);
        }
    }
}
