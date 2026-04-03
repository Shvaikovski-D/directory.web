using FluentValidation;
using directory.web.Infrastructure.Identity;

namespace directory.web.Web.Endpoints;

public class UpdateProfileRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(v => v.FirstName)
            .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов.");

        RuleFor(v => v.LastName)
            .MaximumLength(100).WithMessage("Фамилия не должна превышать 100 символов.");
    }
}