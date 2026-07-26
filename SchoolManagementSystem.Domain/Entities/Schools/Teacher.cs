using SchoolManagementSystem.Domain.Common;

namespace SchoolManagementSystem.Domain.Entities;

public class Teacher : AuditableEntity
{
    public required string Name { get; set; }
    public required string ContactNumber { get; set; }
    public string? Address { get; set; }
}
