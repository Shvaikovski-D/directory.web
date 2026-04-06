using Ardalis.Result;
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace directory.web.Application.Downtimes.Commands.DeleteDowntime;

public record DeleteDowntimeCommand(int Id) : IRequest<Result>;

public class DeleteDowntimeCommandHandler : IRequestHandler<DeleteDowntimeCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteDowntimeCommandHandler> _logger;

    public DeleteDowntimeCommandHandler(
        IApplicationDbContext context,
        ILogger<DeleteDowntimeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteDowntimeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Downtimes
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return Result.NotFound($"Простой с идентификатором {request.Id} не найден.");
            }

            // Hard delete - физическое удаление
            _context.Downtimes.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка при удалении простоя {Id}", request.Id);
            return Result.Error($"Ошибка при удалении простоя: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неожиданная ошибка при удалении простоя {Id}", request.Id);
            return Result.Error($"Неожиданная ошибка: {ex.Message}");
        }
    }
}