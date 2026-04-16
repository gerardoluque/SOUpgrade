using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API.Infrastructure.Audit.Enumerations.Audits;

namespace API.Domain.Audit
{
    public class AuditLog
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }

        public string UserName { get; set; }

        public AuditEventType EventType { get; set; }

        public string? Description { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }
    }
}
