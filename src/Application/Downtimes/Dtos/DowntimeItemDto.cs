using AutoMapper;
using directory.web.Domain.Entities;

namespace directory.web.Application.Downtimes.Dtos;

public class DowntimeItemDto
{
    public int Id { get; init; }

    public int ForkliftId { get; init; }

    public string ForkliftNumber { get; init; } = string.Empty;

    public DateTimeOffset StartedAt { get; init; }

    public DateTimeOffset? EndedAt { get; init; }

    public string Description { get; init; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Downtime, DowntimeItemDto>();
        }
    }
}