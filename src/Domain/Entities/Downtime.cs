namespace directory.web.Domain.Entities;

public class Downtime : BaseAuditableEntity
{
    public int ForkliftId { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset? EndedAt { get; set; }

    public string Description { get; set; } = string.Empty;

    public Forklift Forklift { get; set; } = null!;
}