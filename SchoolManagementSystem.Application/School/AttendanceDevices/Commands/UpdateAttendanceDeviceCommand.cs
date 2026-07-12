using SchoolManagementSystem.Application.School.AttendanceDevices.Models;

namespace SchoolManagementSystem.Application.School.AttendanceDevices.Commands;

public class UpdateAttendanceDeviceCommand : IHttpRequest
{
    public AttendanceDeviceRequest AttendanceDevice { get; set; }
}
