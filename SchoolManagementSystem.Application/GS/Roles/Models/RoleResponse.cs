namespace SchoolManagementSystem.Application.GS.Roles.Models;

public class RoleResponse
{
    public Guid Id { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public Guid? TenantId { get; set; }
    public string? TenantName { get; set; }
    public string? ManagedBy { get; set; }
    public int RoleType { get; set; }
    public string? RoleTypeName { get; set; }
    public bool IsActive { get; set; }
}
