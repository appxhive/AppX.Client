using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Interfaces.Identity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Queries.ForgotPassword
{
    public class ForgotPasswordQueryHandler(ILogger<ForgotPasswordQueryHandler> logger,
        IIdentityService identityService) : IRequestHandler<ForgotPasswordQuery, ApiResponse>
    {
        public async Task<ApiResponse> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Forgot password has been invoked...");

            return await identityService.ForgotPassword();
        }
    }
}
