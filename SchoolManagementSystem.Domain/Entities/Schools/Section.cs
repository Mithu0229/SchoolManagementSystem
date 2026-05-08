namespace SchoolManagementSystem.Domain.Entities;

public class Section : AuditableEntity
{
    public required string SectionName { get; set; }
    public string? Remarks { get; set; }
}
