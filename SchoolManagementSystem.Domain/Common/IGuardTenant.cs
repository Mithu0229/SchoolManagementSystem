namespace SchoolManagementSystem.Domain.Common;
public interface IGuardTenant
{
    Guid? TenantId { get; set; }
}
