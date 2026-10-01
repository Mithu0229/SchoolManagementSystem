namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class StudentBillHistoryResponse
{
    public Guid Id { get; set; }
    public Guid AdmissionId { get; set; }
    public Guid? StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StdCID { get; set; } = string.Empty;
    public int BillMonth { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int BillYear { get; set; }
    public string? VoucherNo { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalPaidAmount { get; set; }
    public decimal TotalDueAmount { get; set; }
    public decimal TotalPaidBkashAmount { get; set; }
    public decimal TotalPaidCashAmount { get; set; }
    public decimal TotalPaidBankAmount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? FormattedTransactionDate { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class StudentBillHistorySummaryResponse
{
    public int TotalBillsCount { get; set; }
    public decimal OverallTotalAmount { get; set; }
    public decimal OverallTotalPaidAmount { get; set; }
    public decimal OverallTotalDueAmount { get; set; }
    public decimal OverallTotalPaidCashAmount { get; set; }
    public decimal OverallTotalPaidBkashAmount { get; set; }
}
