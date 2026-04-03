namespace directory.web.Domain.Entities;

public class Forklift : BaseAuditableEntity
{
    public string Brand { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public decimal LoadCapacity { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }
}
