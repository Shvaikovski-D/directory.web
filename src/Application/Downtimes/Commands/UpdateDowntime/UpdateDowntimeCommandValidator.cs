using FluentValidation;

namespace directory.web.Application.Downtimes.Commands.UpdateDowntime;

public class UpdateDowntimeCommandValidator : AbstractValidator<UpdateDowntimeCommand>
{
    public UpdateDowntimeCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Идентификатор простоя должен быть больше 0.");

        RuleFor(v => v.StartedAt)
            .NotEqual(default(DateTimeOffset)).WithMessage("Дата и время начала простоя обязательны.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Описание простоя обязательно.")
            .MaximumLength(2000).WithMessage("Описание простоя не должно превышать 2000 символов.");
    }
}