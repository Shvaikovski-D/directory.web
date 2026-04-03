using directory.web.Application.Common.Interfaces;
using directory.web.Infrastructure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace directory.web.Web.Endpoints;

public class Users : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapIdentityApi<ApplicationUser>();

        groupBuilder.MapPost(Logout, "logout").RequireAuthorization();
        groupBuilder.MapPut(UpdateProfile, "profile").RequireAuthorization();
    }

    [EndpointSummary("Log out")]
    [EndpointDescription("Logs out the current user by clearing the authentication cookie.")]
    public static async Task<Results<Ok, UnauthorizedHttpResult>> Logout(SignInManager<ApplicationUser> signInManager, [FromBody] object empty)
    {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return TypedResults.Ok();
        }

        return TypedResults.Unauthorized();
    }

    [EndpointSummary("Update user profile")]
    [EndpointDescription("Updates the current user's profile information (first name and last name).")]
    public static async Task<Results<NoContent, UnauthorizedHttpResult>> UpdateProfile(
        [FromServices] IValidator<UpdateProfileRequest> validator,
        [FromBody] UpdateProfileRequest request,
        IUser currentUser,
        UserManager<ApplicationUser> userManager)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.Unauthorized();
        }

        if (string.IsNullOrEmpty(currentUser.Id))
        {
            return TypedResults.Unauthorized();
        }

        var user = await userManager.FindByIdAsync(currentUser.Id);
        if (user == null)
        {
            return TypedResults.Unauthorized();
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        await userManager.UpdateAsync(user);

        return TypedResults.NoContent();
    }
}
