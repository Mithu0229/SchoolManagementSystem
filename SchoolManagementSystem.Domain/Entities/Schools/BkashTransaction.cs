using System;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Domain.Entities.Schools;

public class BkashTransaction : AuditableEntity
{
    public Guid UserId { get; set; }
    public string CustomerNo { get; set; } = string.Empty;
    public string BillMonth { get; set; } = string.Empty;
    public string UserMobileNumber { get; set; } = string.Empty;
    public string TrxId { get; set; } = string.Empty;
    public string PayTime { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Remarks { get; set; } = string.Empty;

}
