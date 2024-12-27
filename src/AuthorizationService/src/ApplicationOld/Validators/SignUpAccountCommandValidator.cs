using ApplicationOld.Commands;
using FluentValidation;

namespace ApplicationOld.Validators;

public class SignUpAccountCommandValidator : AbstractValidator<SignUpAccountCommand>
{
    public SignUpAccountCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}