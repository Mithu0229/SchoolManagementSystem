using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Domain.Entities.Schools;

public class StudentExamMark : AuditableEntity
{
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid StudentId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid ExamComponentId { get; set; }
    public decimal MarksObtained { get; set; }
    public bool IsAbsent { get; set; } = false;
    public string? Remarks { get; set; }

    public AcademicSession? AcademicSession { get; set; }
    public AcademicClass? Class { get; set; }
    public Section? Section { get; set; }
    public StudentInfo? Student { get; set; }
    public Subject? Subject { get; set; }
    public ExamComponent? ExamComponent { get; set; }
}
