using System;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Models;

public class BkashTransactionRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
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
