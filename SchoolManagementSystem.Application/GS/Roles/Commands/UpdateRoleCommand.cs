namespace SchoolManagementSystem.Application.GS.Roles.Commands;
public class UpdateRoleCommand : IHttpRequest
{
    public RoleWithMenuPersmissionRequest RolePermission { get; set; }
    public Guid UserId { get; set; } = Guid.Empty;

}

