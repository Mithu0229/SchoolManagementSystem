namespace SchoolManagementSystem.Application.School.Students.Queries;

public record GetStudentUserListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
