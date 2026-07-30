using System;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Domain.Entities.Schools;

public class BkashTransaction : AuditableEntity
{
    public DateTime Date { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string FromNumber { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
}
