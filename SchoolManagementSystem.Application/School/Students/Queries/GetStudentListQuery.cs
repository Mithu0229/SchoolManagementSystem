namespace SchoolManagementSystem.Application.School.Students.Queries;
public record GetStudentListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
