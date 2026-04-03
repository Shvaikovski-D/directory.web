using AutoMapper.QueryableExtensions;
using directory.web.Application.Common.Interfaces;
using directory.web.Application.Forklifts.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace directory.web.Application.Forklifts.Queries.GetForkliftById;

public record GetForkliftByIdQuery(int Id) : IRequest<ForkliftDto?>;

public class GetForkliftByIdQueryHandler : IRequestHandler<GetForkliftByIdQuery, ForkliftDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetForkliftByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ForkliftDto?> Handle(GetForkliftByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Forklifts
            .AsNoTracking()
            .Where(f => f.Id == request.Id)
            .ProjectTo<ForkliftDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}