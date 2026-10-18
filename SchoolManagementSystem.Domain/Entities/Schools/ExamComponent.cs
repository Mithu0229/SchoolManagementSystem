namespace SchoolManagementSystem.Domain.Entities.Schools;

public class ExamComponent : AuditableEntity
{
    public Guid? AcademicSessionId { get; set; }
    public required string TermName { get; set; } // e.g. "Term-1", "Term-2"
    public required string ComponentName { get; set; } // e.g. "CT-1", "CT-2", "Half-Yearly", "CT-3", "CT-4", "Yearly Exam"
    public decimal MaxMarks { get; set; } // e.g. 15, 70
    public int OrderIndex { get; set; } = 0;

    public AcademicSession? AcademicSession { get; set; }
}
