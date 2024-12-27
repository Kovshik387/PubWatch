using ApplicationOld.Commands;
using FluentValidation;

namespace ApplicationOld.Validators;

public class SignInAccountCommandValidator : AbstractValidator<SignInAccountCommand>
{
    public SignInAccountCommandValidator()
    {
        RuleFor(account => account.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        
        RuleFor(account => account.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}