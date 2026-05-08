namespace SchoolManagementSystem.Domain.Entities;

public class StudentGroup : AuditableEntity
{
    public required string GroupName { get; set; } // Science, Commerce, Arts
    public string? GroupDetails { get; set; }
}
