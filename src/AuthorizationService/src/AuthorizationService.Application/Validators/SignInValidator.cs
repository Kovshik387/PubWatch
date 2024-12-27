using AuthorizationService.Application.Dto;
using FluentValidation;

namespace AuthorizationService.Application.Validators;

public class SignInValidator : AbstractValidator<SignInDto>
{
    public SignInValidator()
    {
        RuleFor(account => account.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        
        RuleFor(account => account.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}