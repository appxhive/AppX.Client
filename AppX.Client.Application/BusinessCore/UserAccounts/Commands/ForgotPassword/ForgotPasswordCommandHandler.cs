using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Interfaces.Identity;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler(
        ILogger<ForgotPasswordCommandHandler> logger,
        IIdentityService identityService,
        IMapper mapper
        ) : IRequestHandler<ForgotPasswordCommand, ApiResponse>
    {
        public async Task<ApiResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Forgot password has been invoked.");

            var user = mapper.Map<ForgotPasswordDto>(request);

            return await identityService.ForgotPassword(user);
        }
    }
}
