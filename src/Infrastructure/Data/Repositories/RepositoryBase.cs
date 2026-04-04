using directory.web.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace directory.web.Infrastructure.Data.Repositories;

public abstract class RepositoryBase<TEntity, TId> : IRepository<TEntity, TId> 
    where TEntity : class
{
    protected readonly IApplicationDbContext _context;

    protected RepositoryBase(IApplicationDbContext context)
    {
        _context = context;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
#nullable disable
        return await _context.Set<TEntity>()
            .FindAsync(new object[] { id }, cancellationToken: cancellationToken)
            .AsTask();
#nullable enable
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}