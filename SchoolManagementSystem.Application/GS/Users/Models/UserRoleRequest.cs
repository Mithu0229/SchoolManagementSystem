namespace SchoolManagementSystem.Application.GS.Users.Models;
public class UserRoleRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? TenantId { get; set; }
    public string? RoleName { get; set; }
    public bool IsActive { get; set; }
    public virtual RoleRequest? Role { get; set; }
}

