using AppX.Client.Domain.Entities.Common;
using MediatR;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.EnableAuthenticator
{
    public class EnableAuthenticatorCommand : IRequest<ApiResponse>
    {
        public string? Code { get; set; }
        public string? Token { get; set; }
        public string? QRCodeUrl { get; set; }
    }
}
