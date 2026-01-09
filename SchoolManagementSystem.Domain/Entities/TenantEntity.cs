using SchoolManagementSystem.Domain.Common;

namespace SchoolManagementSystem.Domain.Entities;
public class TenantEntity : AuditableEntity, IGuardTenant
{
    public Guid? TenantId { get; set; }

}