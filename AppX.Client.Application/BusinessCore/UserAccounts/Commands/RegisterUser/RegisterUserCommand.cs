using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.RegisterUser
{
    public class RegisterUserCommand : IdentityUser, IRequest<bool>
    {
        public Guid? ClientId { get; set; } = default!;
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public required string Password { get; set; }
    }
}
