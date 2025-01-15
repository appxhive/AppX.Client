using FluentValidation;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.LoginUser
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(dto => dto.UserName)
                .NotEmpty().WithMessage("Email address is required.");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.");

        }
    }
}
