namespace SchoolManagementSystem.Application.School.AcademicClasses.Queries;

public record GetAcademicClassListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
