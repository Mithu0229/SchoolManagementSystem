using System;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Models;

public class BkashTransactionRequest
{
    public DateTime Date { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string FromNumber { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
}
