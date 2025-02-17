using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Interfaces.Identity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Queries.SignOut
{
    public class SignOutUserQueryHandler(ILogger<SignOutUserQueryHandler> logger,
        IIdentityService identityService) : IRequestHandler<SignOutUserQuery, ApiResponse>
    {
        public async Task<ApiResponse> Handle(SignOutUserQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Logging out user.");

            return await identityService.SignOutAsync(); 
        }
    }
}
