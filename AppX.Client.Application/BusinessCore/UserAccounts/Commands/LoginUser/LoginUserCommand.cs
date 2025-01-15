using AppX.Client.Domain.Entities.Common;
using MediatR;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<ApiResponse>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
