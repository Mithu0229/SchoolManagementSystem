namespace SchoolManagementSystem.Application.School.AttendanceDevices.Commands;

public record DeleteAttendanceDeviceCommand : IHttpRequest
{
    public Guid Id { get; set; }
}
