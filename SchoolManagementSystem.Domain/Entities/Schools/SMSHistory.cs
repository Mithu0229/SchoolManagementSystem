using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Domain.Entities.Schools;

public class SMSHistory : AuditableEntity
{
    public Guid StudentId { get; set; }
    public required string Message { get; set; }
    public required string Phone { get; set; }
    public required string SMSType { get; set; }
}

public class AttendanceLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public DateTime Punch_time { get; set; }
    public string? Student_code { get; set; }
    public bool IsSent { get; set; }
}

