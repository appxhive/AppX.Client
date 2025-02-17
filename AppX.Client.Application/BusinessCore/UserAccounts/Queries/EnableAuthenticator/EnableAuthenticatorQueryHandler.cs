using AppX.Client.Domain.Interfaces.Identity;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Queries.EnableAuthenticator
{
    public class EnableAuthenticatorQueryHandler(
        ILogger<EnableAuthenticatorQueryHandler> logger,
        IMapper mapper,
        IIdentityService identityService
        ) : IRequestHandler<EnableAuthenticatorQuery, bool>
    {
        public async Task<bool> Handle(EnableAuthenticatorQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Enable Authenticator");

            var result = await identityService.EnableAuthenticatorAsync();

            if (result.Success) return true;

            return false;
        }
    }
}
