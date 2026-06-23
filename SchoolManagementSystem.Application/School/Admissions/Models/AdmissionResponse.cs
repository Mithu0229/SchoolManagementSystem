namespace SchoolManagementSystem.Application.School.Admissions.Models;

public class AdmissionResponse
{
    public Guid Id { get; set; }
    public DateTime AdmissionDate { get; set; }
    public Guid StudentId { get; set; }
    public string? StudentName { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid AcademicSessionId { get; set; }
    public string? SessionName { get; set; }
    public Guid ClassId { get; set; }
    public string? ClassName { get; set; }
    public Guid? SectionId { get; set; }
    public string? SectionName { get; set; }
    public Guid? ShiftId { get; set; }
    public string? ShiftName { get; set; }
    public Guid? GroupId { get; set; }
    public string? GroupName { get; set; }
    public string RollNo { get; set; }
    public decimal MonthlyFeeAmount { get; set; }
    public bool IsPassed { get; set; }
    public bool IsCancelled { get; set; }
    public bool IsActive { get; set; }
}
