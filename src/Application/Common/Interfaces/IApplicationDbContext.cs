using directory.web.Domain.Entities;

namespace directory.web.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Forklift> Forklifts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
