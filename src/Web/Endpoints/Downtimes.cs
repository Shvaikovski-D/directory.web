using directory.web.Application.Common.Models;
using directory.web.Application.Downtimes.Commands.CreateDowntime;
using directory.web.Application.Downtimes.Commands.DeleteDowntime;
using directory.web.Application.Downtimes.Commands.UpdateDowntime;
using directory.web.Application.Downtimes.Dtos;
using directory.web.Application.Downtimes.Queries.GetDowntimeById;
using directory.web.Application.Downtimes.Queries.GetDowntimesByForkliftId;
using directory.web.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace directory.web.Web.Endpoints;

public class Downtimes : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetDowntimesByForkliftId);
        groupBuilder.MapGet(GetDowntimeById, "{id}");
        groupBuilder.MapPost(CreateDowntime);
        groupBuilder.MapPut(UpdateDowntime, "{id}");
        groupBuilder.MapDelete(DeleteDowntime, "{id}");
    }

    [EndpointSummary("Get Downtimes by Forklift ID")]
    [EndpointDescription("Gets all downtimes for a specific forklift, sorted by start date in descending order.")]
    public static async Task<Results<Ok<IEnumerable<DowntimeItemDto>>, BadRequest>> GetDowntimesByForkliftId(
        ISender sender,
        int forkliftId)
    {
        var result = await sender.Send(new GetDowntimesByForkliftIdQuery(forkliftId));
        if (!result.IsSuccess)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Get Downtime by ID")]
    [EndpointDescription("Gets a specific downtime by its ID.")]
    public static async Task<Results<Ok<DowntimeItemDto>, NotFound>> GetDowntimeById(
        ISender sender,
        int id)
    {
        var result = await sender.Send(new GetDowntimeByIdQuery(id));

        if (!result.IsSuccess)
            return TypedResults.NotFound();

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Create a new Downtime")]
    [EndpointDescription("Creates a new downtime using the provided details and returns the ID of the created item.")]
    public static async Task<Results<Created<int>, ValidationProblem, ProblemHttpResult>> CreateDowntime(
        ISender sender, CreateDowntimeCommand command)
    {
        var result = await sender.Send(command);

        return result.ToCreatedResult(
            id => $"/{nameof(Downtimes)}/{id}",
            id => id);
    }

    [EndpointSummary("Update a Downtime")]
    [EndpointDescription("Updates the specified downtime. The ID in the URL must match the ID in the payload.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateDowntime(
        ISender sender,
        int id,
        UpdateDowntimeCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a Downtime")]
    [EndpointDescription("Permanently deletes the downtime with the specified ID from the database. This action cannot be undone.")]
    public static async Task<Results<NoContent, NotFound, ProblemHttpResult>> DeleteDowntime(
        ISender sender,
        int id)
    {
        var result = await sender.Send(new DeleteDowntimeCommand(id));

        return result.ToDeleteResult();
    }
}