using directory.web.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace directory.web.Infrastructure.Data.Configurations;

public class ForkliftConfiguration : IEntityTypeConfiguration<Forklift>
{
    public void Configure(EntityTypeBuilder<Forklift> builder)
    {
        builder.Property(f => f.Brand)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(f => f.Number)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.LoadCapacity)
            .HasPrecision(18, 3)
            .IsRequired();

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true);

        builder.Property(f => f.DeletedAt)
            .IsRequired(false);

        builder.Property(f => f.DeletedBy)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasIndex(f => f.DeletedAt);

        // Глобальный Query Filter для автоматического исключения удаленных записей
        builder.HasQueryFilter(f => f.DeletedAt == null);
    }
}