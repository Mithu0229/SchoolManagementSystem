namespace SchoolManagementSystem.Application.School.Admissions.Queries;

public record GetAdmissionListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
