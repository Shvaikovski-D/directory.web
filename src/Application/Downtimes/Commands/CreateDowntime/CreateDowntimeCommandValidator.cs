using FluentValidation;

namespace directory.web.Application.Downtimes.Commands.CreateDowntime;

public class CreateDowntimeCommandValidator : AbstractValidator<CreateDowntimeCommand>
{
    public CreateDowntimeCommandValidator()
    {
        RuleFor(v => v.ForkliftId)
            .GreaterThan(0).WithMessage("Идентификатор погрузчика должен быть больше 0.");

        RuleFor(v => v.StartedAt)
            .NotEqual(default(DateTimeOffset)).WithMessage("Дата и время начала простоя обязательны.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Описание простоя обязательно.")
            .MaximumLength(2000).WithMessage("Описание простоя не должно превышать 2000 символов.");
    }
}