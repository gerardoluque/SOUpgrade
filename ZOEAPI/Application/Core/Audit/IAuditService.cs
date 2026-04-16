using static API.Infrastructure.Audit.Enumerations.Audits;

namespace API.Application.Core.Audit
{
    public interface IAuditService
    {
        Task LogAsync(AuditEventType eventType, string userId, string userName, string? description = null, string? ipAddress = null, string? userAgent = null);
    }
}
