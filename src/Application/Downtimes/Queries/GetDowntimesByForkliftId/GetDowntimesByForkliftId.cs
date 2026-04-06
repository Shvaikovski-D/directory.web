using Ardalis.Result;
using directory.web.Application.Common.Interfaces;
using directory.web.Application.Downtimes.Dtos;
using directory.web.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace directory.web.Application.Downtimes.Queries.GetDowntimesByForkliftId;

public record GetDowntimesByForkliftIdQuery(int ForkliftId) : IRequest<Result<IEnumerable<DowntimeItemDto>>>;

public class GetDowntimesByForkliftIdQueryHandler : IRequestHandler<GetDowntimesByForkliftIdQuery, Result<IEnumerable<DowntimeItemDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDowntimesByForkliftIdQueryHandler> _logger;

    public GetDowntimesByForkliftIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ILogger<GetDowntimesByForkliftIdQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<DowntimeItemDto>>> Handle(GetDowntimesByForkliftIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var downtimes = await _context.Downtimes
                .Include(d => d.Forklift)
                .Where(d => d.ForkliftId == request.ForkliftId)
                .OrderByDescending(d => d.StartedAt)
                .ToListAsync(cancellationToken);

            var dtoList = _mapper.Map<IEnumerable<DowntimeItemDto>>(downtimes);

            return Result.Success(dtoList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка простоев для погрузчика {ForkliftId}", request.ForkliftId);
            return Result.Error($"Ошибка при получении списка простоев: {ex.Message}");
        }
    }
}