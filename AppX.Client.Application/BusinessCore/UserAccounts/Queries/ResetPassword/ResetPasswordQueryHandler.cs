using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Interfaces.Identity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Queries.ResetPassword
{
    public class ResetPasswordQueryHandler(ILogger<ResetPasswordQueryHandler> logger,
        IIdentityService identityService) : IRequestHandler<ResetPasswordQuery, ApiResponse>
    {
        public async Task<ApiResponse> Handle(ResetPasswordQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Reset password has been requested...");

            return await identityService.ResetPassword(request.Code);
        }
    }
}
