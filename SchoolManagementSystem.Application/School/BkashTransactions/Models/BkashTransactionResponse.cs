using System;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Models;

public class BkashTransactionResponse
{
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMsg { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string FromNumber { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    //
    public string? ConsumerName { get; set; }
    public string? TotalAmount { get; set; }
    public string? TrxId { get; set; }
    public string? MiddlewarePayTime { get; set; }
    public string? RefNumber { get; set; }
    public string? CustomMessage { get; set; }
    public string? AmountBreakdown { get; set; }
}
