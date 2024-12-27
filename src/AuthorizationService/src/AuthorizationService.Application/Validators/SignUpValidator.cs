using AuthorizationService.Application.Dto;
using FluentValidation;

namespace AuthorizationService.Application.Validators;

public class SignUpValidator : AbstractValidator<SignUpDto>
{
    public SignUpValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
        ;
    }
}