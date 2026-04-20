using SOUpgrade.Domain.Enums;

namespace SOUpgrade.Application.Common.DTOs;

public class UpdateServiceOrderDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime? EstimatedCompletionDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}
