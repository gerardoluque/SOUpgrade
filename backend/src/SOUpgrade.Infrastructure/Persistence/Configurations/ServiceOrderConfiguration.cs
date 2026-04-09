using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SOUpgrade.Domain.Entities;

namespace SOUpgrade.Infrastructure.Persistence.Configurations;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.OrderNumber)
            .IsUnique();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.Property(x => x.ClientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ClientEmail)
            .HasMaxLength(200);

        builder.Property(x => x.ClientPhone)
            .HasMaxLength(50);

        builder.Property(x => x.AssignedTo)
            .HasMaxLength(200);

        builder.Property(x => x.Notes)
            .HasMaxLength(4000);

        builder.Property(x => x.Cost)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();
    }
}
