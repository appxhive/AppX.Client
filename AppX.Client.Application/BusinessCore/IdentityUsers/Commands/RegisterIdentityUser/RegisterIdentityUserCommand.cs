using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AppX.Client.Application.BusinessCore.IdentityUsers.Commands.RegisterIdentityUser
{
    public class RegisterIdentityUserCommand : IdentityUser, IRequest<bool>
    {
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Password { get; set; }
    }
}
