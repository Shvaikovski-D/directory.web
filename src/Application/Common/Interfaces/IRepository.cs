namespace directory.web.Application.Common.Interfaces;

public interface IRepository<TEntity, TId> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken);
    
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    void Update(TEntity entity);
    
    void Delete(TEntity entity);
}