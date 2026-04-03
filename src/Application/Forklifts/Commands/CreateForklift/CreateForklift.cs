using directory.web.Application.Common.Exceptions;
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using MediatR;

namespace directory.web.Application.Forklifts.Commands.CreateForklift;

public record CreateForkliftCommand : IRequest<int>
{
    public string Brand { get; init; } = string.Empty;

    public string Number { get; init; } = string.Empty;

    public decimal LoadCapacity { get; init; }
}

public class CreateForkliftCommandHandler : IRequestHandler<CreateForkliftCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateForkliftCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateForkliftCommand request, CancellationToken cancellationToken)
    {
        // Проверяем уникальность номера погрузчика
        var exists = await _context.Forklifts
            .AnyAsync(f => f.Number == request.Number, cancellationToken);

        if (exists)
        {
            throw new ConflictException($"Погрузчик с номером '{request.Number}' уже существует.");
        }

        var entity = new Forklift
        {
            Brand = request.Brand,
            Number = request.Number,
            LoadCapacity = request.LoadCapacity,
            IsActive = true
        };

        _context.Forklifts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}