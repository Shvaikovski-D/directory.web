using AutoMapper;
using AutoMapper.QueryableExtensions;
using directory.web.Application.Common.Interfaces;
using directory.web.Application.Forklifts.Dtos;

namespace directory.web.Application.Forklifts.Queries.GetForkliftById;

public record GetForkliftByIdQuery(int Id) : IRequest<Result<ForkliftItemDto>>;

public class GetForkliftByIdQueryHandler : IRequestHandler<GetForkliftByIdQuery, Result<ForkliftItemDto>>
{
    private readonly IForkliftRepository _forkliftRepository;

    public GetForkliftByIdQueryHandler(IForkliftRepository forkliftRepository)
    {
        _forkliftRepository = forkliftRepository;
    }

    public async Task<Result<ForkliftItemDto>> Handle(GetForkliftByIdQuery request, CancellationToken cancellationToken)
    {
        var forklift = await _forkliftRepository.GetAsync(request.Id, cancellationToken);

        if (forklift == null)
        {
            return Result.NotFound();
        }

        return forklift;
    }
}
