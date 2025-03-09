using AppX.Client.Domain.Entities.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<ApiResponse>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
