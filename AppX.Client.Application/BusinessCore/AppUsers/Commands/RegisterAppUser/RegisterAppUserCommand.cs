using MediatR;

namespace AppX.Client.Application.BusinessCore.AppUsers.Commands.RegisterAppUser
{
    public class RegisterAppUserCommand : IRequest
    {
        public Guid? ClientId { get; set; } = default!;
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
    }
}
