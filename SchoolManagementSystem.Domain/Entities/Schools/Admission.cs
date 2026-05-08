namespace SchoolManagementSystem.Domain.Entities;

public class Admission : AuditableEntity
{
    public DateTime AdmissionDate { get; set; }

    public Guid StudentId { get; set; }
    public Guid BranchId { get; set; }
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid? ShiftId { get; set; }
    public Guid? GroupId { get; set; }

    public required string RollNo { get; set; }
    public bool IsPassed { get; set; }
    public bool IsCancelled { get; set; }

    public Student Student { get; set; }
    public Branch Branch { get; set; }
    public AcademicClass Class { get; set; }
    public Section? Section { get; set; }
    public Shift? Shift { get; set; }
    public StudentGroup? Group { get; set; }
    public AcademicSession AcademicSession { get; set; }
}