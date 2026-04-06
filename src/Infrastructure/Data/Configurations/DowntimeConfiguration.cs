using directory.web.Domain.Entities;
using directory.web.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace directory.web.Infrastructure.Data.Configurations;

public class DowntimeConfiguration : IEntityTypeConfiguration<Downtime>
{
    public void Configure(EntityTypeBuilder<Downtime> builder)
    {
        builder.Property(d => d.StartedAt)
            .IsRequired();

        builder.Property(d => d.EndedAt)
            .IsRequired(false);

        builder.Property(d => d.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasOne(d => d.Forklift)
            .WithMany()
            .HasForeignKey(d => d.ForkliftId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка связи с пользователем, который создал запись
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(d => d.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связи с пользователем, который последним модифицировал запись
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(d => d.LastModifiedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}