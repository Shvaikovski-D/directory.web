using directory.web.Application.Common.Exceptions;
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using MediatR;

namespace directory.web.Application.Forklifts.Commands.UpdateForklift;

public record UpdateForkliftCommand : IRequest
{
    public int Id { get; init; }

    public string Brand { get; init; } = string.Empty;

    public string Number { get; init; } = string.Empty;

    public decimal LoadCapacity { get; init; }

    public bool IsActive { get; init; }
}

public class UpdateForkliftCommandHandler : IRequestHandler<UpdateForkliftCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateForkliftCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateForkliftCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Forklifts
            .FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            throw new Application.Common.Exceptions.NotFoundException(nameof(Forklift), request.Id);
        }

        // Проверяем уникальность номера (исключая текущую запись)
        var exists = await _context.Forklifts
            .AnyAsync(f => f.Number == request.Number && f.Id != request.Id, cancellationToken);

        if (exists)
        {
            throw new ConflictException($"Погрузчик с номером '{request.Number}' уже существует.");
        }

        entity.Brand = request.Brand;
        entity.Number = request.Number;
        entity.LoadCapacity = request.LoadCapacity;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
