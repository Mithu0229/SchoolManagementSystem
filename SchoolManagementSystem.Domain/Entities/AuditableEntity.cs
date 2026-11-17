using SchoolManagementSystem.Domain.Common;

namespace SchoolManagementSystem.Domain.Entities;

public class AuditableEntity:CoreEntity, IAuditable, IGuardTenant
{
    public bool IsActive { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid? ModifiedById { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? TenantId { get; set; }
}
