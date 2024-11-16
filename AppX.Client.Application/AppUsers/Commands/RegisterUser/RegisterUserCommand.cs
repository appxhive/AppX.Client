using MediatR;

namespace AppX.Client.Application.AppUsers.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest
    {
        public Guid? ClientId { get; set; } = default!;
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
    }
}
