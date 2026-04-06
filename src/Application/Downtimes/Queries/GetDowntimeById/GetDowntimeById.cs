using Ardalis.Result;
using directory.web.Application.Common.Interfaces;
using directory.web.Application.Downtimes.Dtos;
using directory.web.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace directory.web.Application.Downtimes.Queries.GetDowntimeById;

public record GetDowntimeByIdQuery(int Id) : IRequest<Result<DowntimeItemDto>>;

public class GetDowntimeByIdQueryHandler : IRequestHandler<GetDowntimeByIdQuery, Result<DowntimeItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDowntimeByIdQueryHandler> _logger;

    public GetDowntimeByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ILogger<GetDowntimeByIdQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<DowntimeItemDto>> Handle(GetDowntimeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var downtime = await _context.Downtimes
                .Include(d => d.Forklift)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (downtime == null)
            {
                return Result.NotFound($"Простой с идентификатором {request.Id} не найден.");
            }

            var dto = _mapper.Map<DowntimeItemDto>(downtime);

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении простоя по идентификатору {Id}", request.Id);
            return Result.Error($"Ошибка при получении простоя: {ex.Message}");
        }
    }
}