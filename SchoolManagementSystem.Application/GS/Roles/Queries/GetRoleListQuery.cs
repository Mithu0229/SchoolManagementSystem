namespace SchoolManagementSystem.Application.GS.Roles.Queries;
public record GetRoleListQuery() : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}