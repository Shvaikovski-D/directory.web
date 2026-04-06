using Ardalis.Result;
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace directory.web.Application.Downtimes.Commands.UpdateDowntime;

public record UpdateDowntimeCommand : IRequest<Result>
{
    public int Id { get; init; }

    public DateTimeOffset StartedAt { get; init; }

    public DateTimeOffset? EndedAt { get; init; }

    public string Description { get; init; } = string.Empty;
}

public class UpdateDowntimeCommandHandler : IRequestHandler<UpdateDowntimeCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateDowntimeCommandHandler> _logger;

    public UpdateDowntimeCommandHandler(
        IApplicationDbContext context,
        ILogger<UpdateDowntimeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateDowntimeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Downtimes
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return Result.NotFound($"Простой с идентификатором {request.Id} не найден.");
            }

            entity.StartedAt = request.StartedAt;
            entity.EndedAt = request.EndedAt;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении простоя {Id}", request.Id);
            return Result.Error($"Ошибка при обновлении простоя: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неожиданная ошибка при обновлении простоя {Id}", request.Id);
            return Result.Error($"Неожиданная ошибка: {ex.Message}");
        }
    }
}