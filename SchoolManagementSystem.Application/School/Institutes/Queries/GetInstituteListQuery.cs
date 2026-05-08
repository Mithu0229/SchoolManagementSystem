namespace SchoolManagementSystem.Application.School.Institutes.Queries;

public record GetInstituteListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
