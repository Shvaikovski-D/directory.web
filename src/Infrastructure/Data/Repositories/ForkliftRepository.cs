using directory.web.Application.Common.Interfaces;
using directory.web.Application.Common.Models;
using directory.web.Application.Forklifts.Dtos;
using directory.web.Domain.Entities;
using directory.web.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace directory.web.Infrastructure.Data.Repositories;

public class ForkliftRepository : RepositoryBase<Forklift, int>, IForkliftRepository
{
    public ForkliftRepository(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<ForkliftItemDto?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var query = from f in _context.Forklifts.AsNoTracking()
                    from u in _context.Set<ApplicationUser>().AsNoTracking()
                        .Where(x => x.Id == f.LastModifiedBy)
                        .DefaultIfEmpty()
                    where f.Id == id
                    select new ForkliftItemDto
                    {
                        Id = f.Id,
                        Brand = f.Brand,
                        Number = f.Number,
                        LoadCapacity = f.LoadCapacity,
                        IsActive = f.IsActive,
                        LastModified = f.LastModified,
                        LastModifiedBy = u != null ? $"{u.LastName} {u.FirstName}".Trim() : string.Empty
                    };

        return await query.FirstOrDefaultAsync();
    }

    public async Task<PagedResultModel<ForkliftItemDto>> GetListAsync(
      string? number, int page, int perPage, CancellationToken cancellationToken)
    {
        page = (page <= 0) ? 1 : page;

        var query = from f in _context.Forklifts.AsNoTracking()
                    join u in _context.Set<ApplicationUser>().AsNoTracking() on f.LastModifiedBy equals u.Id into userJoin
                    from u in userJoin.DefaultIfEmpty()
                    select new ForkliftItemDto
                    {
                        Id = f.Id,
                        Brand = f.Brand,
                        Number = f.Number,
                        LoadCapacity = f.LoadCapacity,
                        IsActive = f.IsActive,
                        LastModified = f.LastModified,
                        LastModifiedBy = u != null ? $"{u.LastName} {u.FirstName}".Trim() : string.Empty
                    };

        if (!string.IsNullOrWhiteSpace(number))
        {
            var searchNumber = number.ToLower();
            query = query.Where(f => EF.Functions.Like(f.Number.ToLower(), $"%{searchNumber}%"));
        }

        int totalCount = await _context.Forklifts.CountAsync();

        var items = await query
            .OrderBy(f => f.Number)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync(cancellationToken);

        int totalPages = (int)Math.Ceiling(totalCount / (double)perPage);
        var result = new PagedResultModel<ForkliftItemDto>(items, page, perPage, totalCount, totalPages);

        return result;
    }
}
