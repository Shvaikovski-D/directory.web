using directory.web.Application.Common.Models;

namespace directory.web.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(SimpleResult Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<SimpleResult> DeleteUserAsync(string userId);
}
