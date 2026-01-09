namespace SchoolManagementSystem.Application.GS.Users.Models;
public class UserRoleResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public Guid RoleId { get; set; }
    public string? RoleName { get; set; }
    public Guid? TenantId { get; set; }
    public string? TenantName { get; set; }

}