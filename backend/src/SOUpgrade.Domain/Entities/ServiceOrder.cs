using SOUpgrade.Domain.Enums;

namespace SOUpgrade.Domain.Entities;

public class ServiceOrder
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ServiceOrderStatus Status { get; set; } = ServiceOrderStatus.Pending;
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime? EstimatedCompletionDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}
