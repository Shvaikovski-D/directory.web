using directory.web.Application.Forklifts.Commands.CreateForklift;
using directory.web.Application.Forklifts.Commands.DeleteForklift;
using directory.web.Application.Forklifts.Commands.HardDeleteForklift;
using directory.web.Application.Forklifts.Commands.RestoreForklift;
using directory.web.Application.Forklifts.Commands.UpdateForklift;
using directory.web.Application.Forklifts.Dtos;
using directory.web.Application.Forklifts.Queries.GetForkliftById;
using directory.web.Application.Forklifts.Queries.GetForklifts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace directory.web.Web.Endpoints;

public class Forklifts : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetForklifts);
        groupBuilder.MapGet(GetForkliftById, "{id}");
        groupBuilder.MapPost(CreateForklift);
        groupBuilder.MapPut(UpdateForklift, "{id}");
        groupBuilder.MapDelete(DeleteForklift, "{id}");
        groupBuilder.MapPost(RestoreForklift, "{id}/restore");
        groupBuilder.MapDelete(HardDeleteForklift, "{id}/hard-delete");
    }

    [EndpointSummary("Get all Forklifts")]
    [EndpointDescription("Gets all forklifts with optional search by number. Search is case-insensitive and matches partial strings.")]
    public static async Task<Ok<List<ForkliftDto>>> GetForklifts(
        ISender sender,
        string? searchNumber = null)
    {
        var result = await sender.Send(new GetForkliftsQuery(searchNumber));
        return TypedResults.Ok(result);
    }

    [EndpointSummary("Get Forklift by ID")]
    [EndpointDescription("Gets a specific forklift by its ID.")]
    public static async Task<Results<Ok<ForkliftDto>, NotFound>> GetForkliftById(
        ISender sender,
        int id)
    {
        var result = await sender.Send(new GetForkliftByIdQuery(id));

        if (result is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(result);
    }

    [EndpointSummary("Create a new Forklift")]
    [EndpointDescription("Creates a new forklift using the provided details and returns the ID of the created item.")]
    public static async Task<Created<int>> CreateForklift(ISender sender, CreateForkliftCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Forklifts)}/{id}", id);
    }

    [EndpointSummary("Update a Forklift")]
    [EndpointDescription("Updates the specified forklift. The ID in the URL must match the ID in the payload.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateForklift(
        ISender sender,
        int id,
        UpdateForkliftCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a Forklift")]
    [EndpointDescription("Performs soft delete of the forklift with the specified ID. The record remains in the database but is marked as deleted.")]
    public static async Task<NoContent> DeleteForklift(ISender sender, int id)
    {
        await sender.Send(new DeleteForkliftCommand(id));

        return TypedResults.NoContent();
    }

    [EndpointSummary("Restore a Forklift")]
    [EndpointDescription("Restores a previously soft-deleted forklift with the specified ID.")]
    public static async Task<NoContent> RestoreForklift(ISender sender, int id)
    {
        await sender.Send(new RestoreForkliftCommand(id));

        return TypedResults.NoContent();
    }

    [EndpointSummary("Hard Delete a Forklift")]
    [EndpointDescription("Permanently deletes the forklift with the specified ID from the database. This action cannot be undone.")]
    public static async Task<NoContent> HardDeleteForklift(ISender sender, int id)
    {
        await sender.Send(new HardDeleteForkliftCommand(id));

        return TypedResults.NoContent();
    }
}
