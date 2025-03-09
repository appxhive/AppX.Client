using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Interfaces.Identity;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler(
        ILogger<ResetPasswordCommandHandler> logger,
        IIdentityService identityService,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<ResetPasswordCommand, ApiResponse>
    {
        public async Task<ApiResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Reset password has been requested for user.");

            //var queryString = httpContextAccessor.HttpContext.Request.QueryString.Value;

            var user = mapper.Map<ResetPasswordDto>(request);

            var response = await identityService.ResetPassword(user);

            return response;
        }
    }
}
