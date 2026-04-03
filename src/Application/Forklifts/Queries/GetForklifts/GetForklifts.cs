using AutoMapper;
using AutoMapper.QueryableExtensions;
using directory.web.Application.Common.Interfaces;
using directory.web.Application.Forklifts.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace directory.web.Application.Forklifts.Queries.GetForklifts;

public record GetForkliftsQuery(string? SearchNumber = null) : IRequest<List<ForkliftDto>>;

public class GetForkliftsQueryHandler : IRequestHandler<GetForkliftsQuery, List<ForkliftDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetForkliftsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ForkliftDto>> Handle(GetForkliftsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Forklifts
            .AsNoTracking();

        // Поиск по номеру погрузчика - поиск по вхождению строки без учета регистра
        if (!string.IsNullOrWhiteSpace(request.SearchNumber))
        {
            var searchNumber = request.SearchNumber.ToLower();
            query = query.Where(f => EF.Functions.Like(f.Number.ToLower(), $"%{searchNumber}%"));
        }

        return await query
            .OrderBy(f => f.Number)
            .ProjectTo<ForkliftDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}