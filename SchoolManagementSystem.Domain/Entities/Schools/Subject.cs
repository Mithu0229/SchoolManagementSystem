namespace SchoolManagementSystem.Domain.Entities.Schools;

public class Subject : AuditableEntity
{
    public required string SubjectName { get; set; }
    public string? SubjectCode { get; set; }
    public string? Description { get; set; }
}
