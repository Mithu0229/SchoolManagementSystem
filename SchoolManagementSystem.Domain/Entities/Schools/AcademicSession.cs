namespace SchoolManagementSystem.Domain.Entities;

public class AcademicSession : AuditableEntity
{
    public required string SessionName { get; set; } // 2025-2026
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsCurrent { get; set; }
}
