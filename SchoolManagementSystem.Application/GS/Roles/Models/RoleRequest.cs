namespace SchoolManagementSystem.Application.GS.Roles.Models;

public class RoleRequest
{
    public Guid Id { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public Guid? TenantId { get; set; }
    public int RoleType { get; set; }
    public bool IsActive { get; set; }
}
