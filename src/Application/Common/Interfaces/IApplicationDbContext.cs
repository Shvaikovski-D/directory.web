  using directory.web.Domain.Entities;
  using Microsoft.EntityFrameworkCore;

  namespace directory.web.Application.Common.Interfaces;

  public interface IApplicationDbContext
  {
      DbSet<TodoList> TodoLists { get; }

      DbSet<TodoItem> TodoItems { get; }

      DbSet<Forklift> Forklifts { get; }

      DbSet<TEntity> Set<TEntity>() where TEntity : class;

      Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
