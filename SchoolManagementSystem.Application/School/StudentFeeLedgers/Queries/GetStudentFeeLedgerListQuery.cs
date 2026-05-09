namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Queries;

public record GetStudentFeeLedgerListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
