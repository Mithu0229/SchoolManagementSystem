namespace SchoolManagementSystem.Application.GS.Roles.Commands;
public class InsertRoleCommand : IHttpRequest
{
    public RoleWithMenuPersmissionRequest? RolePermission { get; set; }
    public Guid UserId { get; set; } = Guid.Empty;

}