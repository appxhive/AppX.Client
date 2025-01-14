using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Interfaces.Identity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Queries.GetUserConfirmation
{
    public class GetUserConfirmationQueryHandler(ILogger<GetUserConfirmationQueryHandler> logger,
        IIdentityService identityService) : IRequestHandler<GetUserConfirmationQuery, ApiResponse>
    {
        public async Task<ApiResponse> Handle(GetUserConfirmationQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Get user confirmation email.");

            return await identityService.ConfirmEmailAsync(request.UserId, request.Token);
        }
    }
}
