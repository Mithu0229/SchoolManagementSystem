namespace SchoolManagementSystem.Application.GS.Roles.Models;
public class RoleWithMenuPersmissionRequest
{
    public RoleRequest? Role { get; set; }
    public List<RoleMenuRequest> RoleMenus { get; set; } = new List<RoleMenuRequest>();
}