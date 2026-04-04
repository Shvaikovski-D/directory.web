using directory.web.Application.Common;
using directory.web.Application.Common.Interfaces;
using directory.web.Application.Common.Models;
using directory.web.Application.Forklifts.Dtos;

namespace directory.web.Application.Forklifts.Queries.GetForklifts;

public record GetForkliftsQuery(string? SearchNumber = null, int? Page = Constants.DEFAULT_PAGE, int? PerPage = Constants.DEFAULT_PAGE_SIZE)
    : IRequest<Result<PagedResultModel<ForkliftItemDto>>>;

public class GetForkliftsQueryHandler : IRequestHandler<GetForkliftsQuery, Result<PagedResultModel<ForkliftItemDto>>>
{
    private readonly IForkliftRepository _forkliftRepository;
    private readonly IMapper _mapper;

    public GetForkliftsQueryHandler(IForkliftRepository forkliftRepository, IMapper mapper)
    {
        _forkliftRepository = forkliftRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResultModel<ForkliftItemDto>>> Handle(GetForkliftsQuery request, CancellationToken cancellationToken)
    {
        var result = await _forkliftRepository.GetListAsync(
            request.SearchNumber,
            request.Page ?? Constants.DEFAULT_PAGE,
            request.PerPage ?? Constants.DEFAULT_PAGE_SIZE,
            cancellationToken);

        return Result.Success(result);
    }
}
