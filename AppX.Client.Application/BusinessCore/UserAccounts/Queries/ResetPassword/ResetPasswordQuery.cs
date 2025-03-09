using AppX.Client.Domain.Entities.Common;
using MediatR;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Queries.ResetPassword
{
    public class ResetPasswordQuery : IRequest<ApiResponse>
    {
        public string Code { get; set; }
    }
}
