namespace SchoolManagementSystem.Application.School.StudentGroups.Queries;

public record GetStudentGroupListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
