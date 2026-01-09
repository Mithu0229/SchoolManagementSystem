namespace SchoolManagementSystem.Application.GS.Users.Queries;
public record GetUserListQuery() : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}