namespace SchoolManagementSystem.Application.School.Shifts.Queries;

public record GetShiftListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
