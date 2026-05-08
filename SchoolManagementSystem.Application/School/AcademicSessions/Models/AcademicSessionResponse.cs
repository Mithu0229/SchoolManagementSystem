namespace SchoolManagementSystem.Application.School.AcademicSessions.Models;

public class AcademicSessionResponse
{
    public Guid Id { get; set; }
    public string SessionName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsActive { get; set; }
}
