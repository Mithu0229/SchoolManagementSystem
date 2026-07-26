using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Domain.Entities.Schools;

public class SMSHistory : AuditableEntity
{
    public Guid StudentId { get; set; }
    public required string Message { get; set; }
    public required string Phone { get; set; }
    public required string SMSType { get; set; }
}

