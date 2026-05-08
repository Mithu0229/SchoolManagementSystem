namespace SchoolManagementSystem.Application.School.AcademicSessions.Queries;

public record GetAcademicSessionListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
