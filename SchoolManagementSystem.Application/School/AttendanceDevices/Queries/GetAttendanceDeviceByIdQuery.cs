namespace SchoolManagementSystem.Application.School.AttendanceDevices.Queries;

public record GetAttendanceDeviceByIdQuery : IHttpRequest
{
    public Guid Id { get; set; }
}
