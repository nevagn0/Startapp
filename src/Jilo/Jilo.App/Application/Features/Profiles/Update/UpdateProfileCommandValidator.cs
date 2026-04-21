using FluentValidation;

namespace Jilo.App.Application.Features.Profiles.Update;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    private const int BioMaxLength = 512;

    public UpdateProfileCommandValidator()
    {
        When(c => !string.IsNullOrEmpty(c.Bio), () =>
        {
            RuleFor(c => c.Bio)
                .MaximumLength(BioMaxLength).WithMessage($"Maximum bio length is {BioMaxLength}");
        });
    }
}
