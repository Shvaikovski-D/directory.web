using AutoMapper;
using directory.web.Domain.Entities;

namespace directory.web.Application.Forklifts.Dtos;

public class ForkliftItemDto
{
    public int Id { get; init; }

    public string Brand { get; init; } = string.Empty;

    public string Number { get; init; } = string.Empty;

    public decimal LoadCapacity { get; init; }

    public bool IsActive { get; init; }

    public DateTimeOffset LastModified { get; init; }

    public string? LastModifiedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Forklift, ForkliftItemDto>();
        }
    }
}
