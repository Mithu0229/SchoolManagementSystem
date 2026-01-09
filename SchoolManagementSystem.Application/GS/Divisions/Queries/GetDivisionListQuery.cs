namespace SchoolManagementSystem.Application.GS.Divisions.Queries;
public record GetDivisionListQuery() : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}