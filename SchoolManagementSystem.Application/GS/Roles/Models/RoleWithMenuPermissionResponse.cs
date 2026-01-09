namespace SchoolManagementSystem.Application.GS.Roles.Models;

public class RoleWithMenuPermissionResponse
{
    public RoleResponse? Role { get; set; }
    public List<RoleMenuResponse> RoleMenus { get; set; } = new List<RoleMenuResponse>();
}