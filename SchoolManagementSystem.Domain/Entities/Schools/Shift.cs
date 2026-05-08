namespace SchoolManagementSystem.Domain.Entities;

public class Shift : AuditableEntity
{
    public required string ShiftName { get; set; } // Morning, Day
}
