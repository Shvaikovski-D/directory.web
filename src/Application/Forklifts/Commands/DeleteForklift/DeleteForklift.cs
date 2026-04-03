using directory.web.Application.Common.Exceptions;
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using MediatR;

namespace directory.web.Application.Forklifts.Commands.DeleteForklift;

public record DeleteForkliftCommand(int Id) : IRequest;

public class DeleteForkliftCommandHandler : IRequestHandler<DeleteForkliftCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public DeleteForkliftCommandHandler(IApplicationDbContext context, IUser user, IIdentityService identityService)
    {
        _context = context;
        _user = user;
        _identityService = identityService;
    }

    public async Task Handle(DeleteForkliftCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Forklifts
            .FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            throw new Application.Common.Exceptions.NotFoundException(nameof(Forklift), request.Id);
        }

        // Soft Delete - устанавливаем дату удаления и пользователя
        entity.DeletedAt = DateTimeOffset.UtcNow;
        if (_user.Id != null)
        {
            entity.DeletedBy = await _identityService.GetUserNameAsync(_user.Id);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
