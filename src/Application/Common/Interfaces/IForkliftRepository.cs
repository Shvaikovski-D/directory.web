using directory.web.Application.Forklifts.Dtos;
using directory.web.Domain.Entities;

namespace directory.web.Application.Common.Interfaces;

public interface IForkliftRepository : IRepository<Forklift, int>
{
    Task<ForkliftItemDto?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Models.PagedResultModel<ForkliftItemDto>> GetListAsync(
      string? number, int page, int perPage, CancellationToken cancellationToken);
}
