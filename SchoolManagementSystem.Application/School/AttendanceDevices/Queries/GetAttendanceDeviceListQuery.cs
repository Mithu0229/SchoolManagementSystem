namespace SchoolManagementSystem.Application.School.AttendanceDevices.Queries;

public record GetAttendanceDeviceListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
