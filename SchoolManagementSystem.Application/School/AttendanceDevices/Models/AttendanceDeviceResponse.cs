namespace SchoolManagementSystem.Application.School.AttendanceDevices.Models;

public class AttendanceDeviceResponse
{
    public Guid Id { get; set; }
    public string DeviceNo { get; set; }
    public string CardNo { get; set; }
    public DateTime DtPunchDate { get; set; }
    public DateTime DtPunchTime { get; set; }
    public bool InOut { get; set; }
    public bool IsActive { get; set; }
}
