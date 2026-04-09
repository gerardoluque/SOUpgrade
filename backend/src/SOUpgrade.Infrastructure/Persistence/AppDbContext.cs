using Microsoft.EntityFrameworkCore;
using SOUpgrade.Domain.Entities;
using SOUpgrade.Domain.Enums;

namespace SOUpgrade.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ServiceOrder> ServiceOrders => Set<ServiceOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<ServiceOrder>().HasData(
            new ServiceOrder
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                OrderNumber = "SO-20240101-SEED01",
                Title = "Network Infrastructure Setup",
                Description = "Configure and setup company network infrastructure",
                Status = ServiceOrderStatus.InProgress,
                Priority = Priority.High,
                ClientName = "Acme Corporation",
                ClientEmail = "it@acme.com",
                ClientPhone = "+1-555-0100",
                AssignedTo = "John Technician",
                EstimatedCompletionDate = now.AddDays(7),
                CompletedAt = null,
                Notes = "Requires access to server room",
                Cost = 2500.00m,
                CreatedAt = now,
                UpdatedAt = now
            },
            new ServiceOrder
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                OrderNumber = "SO-20240101-SEED02",
                Title = "Printer Maintenance",
                Description = "Monthly maintenance for office printers",
                Status = ServiceOrderStatus.Pending,
                Priority = Priority.Low,
                ClientName = "Tech Solutions Ltd",
                ClientEmail = "admin@techsolutions.com",
                ClientPhone = "+1-555-0200",
                AssignedTo = "Jane Technician",
                EstimatedCompletionDate = now.AddDays(3),
                CompletedAt = null,
                Notes = "3 printers on floor 2",
                Cost = 350.00m,
                CreatedAt = now,
                UpdatedAt = now
            },
            new ServiceOrder
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                OrderNumber = "SO-20240101-SEED03",
                Title = "Server Upgrade",
                Description = "Upgrade main server RAM and storage",
                Status = ServiceOrderStatus.Completed,
                Priority = Priority.Critical,
                ClientName = "Global Industries",
                ClientEmail = "support@global.com",
                ClientPhone = "+1-555-0300",
                AssignedTo = "Bob Engineer",
                EstimatedCompletionDate = now.AddDays(-5),
                CompletedAt = now.AddDays(-6),
                Notes = "Completed ahead of schedule",
                Cost = 8900.00m,
                CreatedAt = now.AddDays(-10),
                UpdatedAt = now.AddDays(-6)
            }
        );
    }
}
