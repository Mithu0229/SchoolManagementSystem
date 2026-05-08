namespace SchoolManagementSystem.Application.School.Sections.Queries;

public record GetSectionListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
