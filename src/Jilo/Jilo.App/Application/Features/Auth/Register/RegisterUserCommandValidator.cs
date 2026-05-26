using FluentValidation;

namespace Jilo.App.Application.Features.Auth.Register;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private const int MinPasswordLength = 5;

    private const int MinUsernameLength = 4;

    private const int MaxUsernameLength = 20;

    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(MinPasswordLength).WithMessage($"Password must be at least {MinPasswordLength} characters")
            .Must(ContainAtLeastOneDigit).WithMessage("At least one digit is required")
            .Must(ContainAtLeastOneUpperCharacter).WithMessage("At least one upper character is required");

        RuleFor(c => c.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(MinUsernameLength).WithMessage($"Min username length is {MinUsernameLength}")
            .MaximumLength(MaxUsernameLength).WithMessage($"Max username length is {MaxUsernameLength}");
    }

    private static bool ContainAtLeastOneDigit(string? str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }

        return str.Any(char.IsDigit);
    }

    private static bool ContainAtLeastOneUpperCharacter(string? str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }

        return str.Any(char.IsUpper);
    }
}
