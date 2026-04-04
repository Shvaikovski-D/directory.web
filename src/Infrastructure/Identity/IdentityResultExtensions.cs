using directory.web.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace directory.web.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static SimpleResult ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? SimpleResult.Success()
            : SimpleResult.Failure(result.Errors.Select(e => e.Description));
    }
}
