using SchoolManagementSystem.Application.School.AttendanceDevices.Models;

namespace SchoolManagementSystem.Application.School.AttendanceDevices.Commands;

public class InsertAttendanceDeviceCommand : IHttpRequest
{
    public AttendanceDeviceRequest AttendanceDevice { get; set; }
}
