using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Interfaces.Identity;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.EnableAuthenticator
{
    public class EnableAuthenticatorCommandHandler(
          ILogger<EnableAuthenticatorCommandHandler> logger,
          IIdentityService identityService,
          IMapper mapper
        )
        : IRequestHandler<EnableAuthenticatorCommand, ApiResponse>
    {
        public async Task<ApiResponse> Handle(EnableAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            var twoFactorAuth = mapper.Map<TwoFactorAuthentication>(request);

            return await identityService.EnableAuthenticatorAsync(twoFactorAuth);
        }
    }
}
