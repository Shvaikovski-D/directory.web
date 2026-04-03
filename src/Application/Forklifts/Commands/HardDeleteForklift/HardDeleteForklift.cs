using directory.web.Application.Common.Exceptions;
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace directory.web.Application.Forklifts.Commands.HardDeleteForklift;

public record HardDeleteForkliftCommand(int Id) : IRequest;

public class HardDeleteForkliftCommandHandler : IRequestHandler<HardDeleteForkliftCommand>
{
    private readonly IApplicationDbContext _context;

    public HardDeleteForkliftCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(HardDeleteForkliftCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Forklifts
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new Application.Common.Exceptions.NotFoundException(nameof(Forklift), request.Id);
        }

        // Hard Delete - физическое удаление записи из базы данных
        _context.Forklifts.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}