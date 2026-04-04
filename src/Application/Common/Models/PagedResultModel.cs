namespace directory.web.Application.Common.Models;

public record PagedResultModel<T>(
  IReadOnlyList<T> Items,
  int Page,
  int PerPage,
  int TotalCount,
  int TotalPages);
