namespace SchoolManagementSystem.Application.School.Admissions.Models;

public class AdmissionRequest
{
    public Guid Id { get; set; }
    public DateTime AdmissionDate { get; set; }
    public Guid StudentId { get; set; }
    public Guid BranchId { get; set; }
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid? ShiftId { get; set; }
    public Guid? GroupId { get; set; }
    public Guid? TeacherId { get; set; }
    public string RollNo { get; set; }
    public decimal MonthlyFeeAmount { get; set; }
    public bool IsPassed { get; set; }
    public bool IsCancelled { get; set; }
    public bool IsActive { get; set; }
}
