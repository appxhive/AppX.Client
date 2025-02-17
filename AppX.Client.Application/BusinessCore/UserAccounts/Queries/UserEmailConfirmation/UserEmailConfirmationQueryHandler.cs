using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Interfaces.Identity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Queries.GetUserConfirmation
{
    public class UserEmailConfirmationQueryHandler(ILogger<UserEmailConfirmationQueryHandler> logger,
        IIdentityService identityService) : IRequestHandler<UserEmailConfirmationQuery, ApiResponse>
    {
        public async Task<ApiResponse> Handle(UserEmailConfirmationQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Get user confirmation email.");

            return await identityService.ConfirmEmailAsync(request.UserId, request.Token);
        }
    }
}
