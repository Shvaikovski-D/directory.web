using FluentValidation;

namespace directory.web.Application.Forklifts.Commands.CreateForklift;

public class CreateForkliftCommandValidator : AbstractValidator<CreateForkliftCommand>
{
    public CreateForkliftCommandValidator()
    {
        RuleFor(v => v.Brand)
            .NotEmpty().WithMessage("Марка погрузчика обязательна.")
            .MaximumLength(200).WithMessage("Марка погрузчика не должна превышать 200 символов.");

        RuleFor(v => v.Number)
            .NotEmpty().WithMessage("Номер погрузчика обязателен.")
            .MaximumLength(100).WithMessage("Номер погрузчика не должен превышать 100 символов.");

        RuleFor(v => v.LoadCapacity)
            .GreaterThan(0).WithMessage("Грузоподъемность должна быть больше 0.");
    }
}