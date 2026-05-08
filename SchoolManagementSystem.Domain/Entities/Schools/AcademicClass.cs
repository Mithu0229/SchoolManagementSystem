namespace SchoolManagementSystem.Domain.Entities;

public class AcademicClass : AuditableEntity
{
    public required string ClassName { get; set; }
    public string? ClassDetails { get; set; }
}
