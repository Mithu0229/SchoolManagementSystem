namespace SchoolManagementSystem.Application.School.SchoolStudents.Queries;

public record GetStudentListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
