using directory.web.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace directory.web.Web.Infrastructure;

/// <summary>
/// Converts well-known application exceptions into RFC 9110-compliant <see cref="ProblemDetails"/> responses,
/// mapping <see cref="ValidationException"/> → 400, <see cref="NotFoundException"/> → 404,
/// <see cref="ConflictException"/> → 409, <see cref="UnauthorizedAccessException"/> → 401,
/// and <see cref="ForbiddenAccessException"/> → 403.
/// Also handles EF Core exceptions appropriately.
/// Unrecognised exceptions are not handled and fall through to the default middleware.
/// </summary>
public class ProblemDetailsExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, problemDetails) = exception switch
        {
            ValidationException ve => (StatusCodes.Status400BadRequest, (ProblemDetails)new ValidationProblemDetails(ve.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            }),
            Application.Common.Exceptions.NotFoundException ne => (StatusCodes.Status404NotFound, new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "The specified resource was not found.",
                Detail = ne.Message
            }),
            ConflictException ce => (StatusCodes.Status409Conflict, new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                Title = "Conflict",
                Detail = ce.Message
            }),
            DbUpdateConcurrencyException ce => (StatusCodes.Status409Conflict, new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                Title = "Concurrency Conflict",
                Detail = "Ресурс был изменен другим пользователем. Пожалуйста, обновите данные и попробуйте снова."
            }),
            DbUpdateException dbe => HandleDbUpdateException(dbe),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.2"
            }),
            ForbiddenAccessException => (StatusCodes.Status403Forbidden, new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.4"
            }),
            _ => (-1, null)
        };

        if (problemDetails is null) return false;

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static (int statusCode, ProblemDetails? problemDetails) HandleDbUpdateException(DbUpdateException exception)
    {
        // Проверяем на нарушение уникальности (PostgreSQL error code 23505)
        if (exception.InnerException?.Message.Contains("23505") == true || 
            exception.InnerException?.Message.Contains("duplicate key") == true ||
            exception.InnerException?.Message.Contains("уникальное ограничение") == true)
        {
            return (StatusCodes.Status409Conflict, new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                Title = "Conflict",
                Detail = "Нарушение уникальности данных. Возможно, запись с таким значением уже существует."
            });
        }

        // Проверяем на нарушение внешнего ключа (PostgreSQL error code 23503)
        if (exception.InnerException?.Message.Contains("23503") == true ||
            exception.InnerException?.Message.Contains("foreign key") == true ||
            exception.InnerException?.Message.Contains("внешнего ключа") == true)
        {
            return (StatusCodes.Status400BadRequest, new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Bad Request",
                Detail = "Нарушение ссылочной целостности. Возможно, указана несуществующая связанная запись."
            });
        }

        // Для остальных ошибок базы данных возвращаем null, чтобы упало в 500
        return (-1, null);
    }
}
