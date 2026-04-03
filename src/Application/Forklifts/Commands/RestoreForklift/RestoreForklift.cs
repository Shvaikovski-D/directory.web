using directory.web.Application.Common.Exceptions;
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace directory.web.Application.Forklifts.Commands.RestoreForklift;

public record RestoreForkliftCommand(int Id) : IRequest;

public class RestoreForkliftCommandHandler : IRequestHandler<RestoreForkliftCommand>
{
    private readonly IApplicationDbContext _context;

    public RestoreForkliftCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(RestoreForkliftCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Forklifts
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new Application.Common.Exceptions.NotFoundException(nameof(Forklift), request.Id);
        }

        if (entity.DeletedAt == null)
        {
            throw new ConflictException($"Погрузчик с ID {request.Id} не был удален.");
        }

        // Восстановление записи
        entity.DeletedAt = null;
        entity.DeletedBy = null;

        await _context.SaveChangesAsync(cancellationToken);
    }
}