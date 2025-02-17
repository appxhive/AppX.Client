using AppX.Client.Domain.Entities.Common;
using MediatR;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Queries.GetUserConfirmation
{
    public class UserEmailConfirmationQuery(string userId, string token) : IRequest<ApiResponse>
    {
        public string UserId { get; } = userId;
        public string Token { get; } = token;
    }
}
