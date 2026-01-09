namespace SchoolManagementSystem.Application.GS.Roles.Queries;
public class GetRolesPagesQuery : IHttpRequest
{
    public PagedRequest? RolesPaged { get; set; }

}