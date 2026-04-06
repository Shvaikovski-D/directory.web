using Ardalis.Result;
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace directory.web.Application.Downtimes.Commands.CreateDowntime;

public record CreateDowntimeCommand : IRequest<Result<int>>
{
    public int ForkliftId { get; init; }

    public DateTimeOffset StartedAt { get; init; }

    public DateTimeOffset? EndedAt { get; init; }

    public string Description { get; init; } = string.Empty;
}

public class CreateDowntimeCommandHandler : IRequestHandler<CreateDowntimeCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateDowntimeCommandHandler> _logger;

    public CreateDowntimeCommandHandler(
        IApplicationDbContext context,
        ILogger<CreateDowntimeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(CreateDowntimeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Проверяем существование погрузчика
            var forkliftExists = await _context.Forklifts
                .AnyAsync(f => f.Id == request.ForkliftId, cancellationToken);

            if (!forkliftExists)
            {
                return Result.NotFound($"Погрузчик с идентификатором {request.ForkliftId} не найден.");
            }

            var entity = new Downtime
            {
                ForkliftId = request.ForkliftId,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                Description = request.Description
            };

            _context.Downtimes.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(entity.Id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка при создании простоя для погрузчика {ForkliftId}", request.ForkliftId);
            return Result.Error($"Ошибка при создании простоя: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неожиданная ошибка при создании простоя для погрузчика {ForkliftId}", request.ForkliftId);
            return Result.Error($"Неожиданная ошибка: {ex.Message}");
        }
    }
}