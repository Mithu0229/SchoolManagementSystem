namespace SchoolManagementSystem.Domain.Entities;

public class AttendanceDevice : AuditableEntity
{
    public required string DeviceNo { get; set; }
    public required string CardNo { get; set; }
    public DateTime DtPunchDate { get; set; }
    public DateTime DtPunchTime { get; set; }
    public bool InOut { get; set; }
}
