using FluentValidation;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.RegisterUser
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password length must be at least 8.")
                .MaximumLength(32).WithMessage("Password length must not exceed 32 characters.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        }
    }
}
