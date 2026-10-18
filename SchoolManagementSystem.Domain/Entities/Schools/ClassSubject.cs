namespace SchoolManagementSystem.Domain.Entities.Schools;

public class ClassSubject : AuditableEntity
{
    public Guid ClassId { get; set; }
    public Guid SubjectId { get; set; }
    public int OrderIndex { get; set; } = 0;

    public AcademicClass? Class { get; set; }
    public Subject? Subject { get; set; }
}
