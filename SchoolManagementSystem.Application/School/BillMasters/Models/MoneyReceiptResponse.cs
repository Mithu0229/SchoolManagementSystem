namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class MoneyReceiptResponse
{
    public Guid BillMasterId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentID { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public int ManualMR { get; set; }
    public string InvID { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; } // Total Billed Amount
    public decimal TotalBilledAmount { get; set; }
    public decimal CashAmount { get; set; }
    public decimal BankAmount { get; set; }
    public decimal BkashAmount { get; set; }
    public decimal TotalPaidAmount { get; set; }
    public decimal TotalDueAmount { get; set; }
    public int BillMonth { get; set; }
    public int BillYear { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public string VoucherNo { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string Inword { get; set; } = string.Empty;
    public List<MoneyReceiptDetailResponse> Details { get; set; } = new List<MoneyReceiptDetailResponse>();
    public List<MoneyReceiptPaymentRowResponse> PaymentRows { get; set; } = new List<MoneyReceiptPaymentRowResponse>();
}

public class MoneyReceiptDetailResponse
{
    public int SN { get; set; }
    public string AccountHead { get; set; } = string.Empty;
    public string Month { get; set; } = string.Empty;
    public string TransID { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class MoneyReceiptPaymentRowResponse
{
    public int SN { get; set; }
    public string Source { get; set; } = string.Empty; // CashBook or BankBook
    public string PaymentMethod { get; set; } = string.Empty; // Cash, bKash, Bank
    public string AccountOrBank { get; set; } = string.Empty;
    public string TrxNoOrVoucher { get; set; } = string.Empty;
    public string TransactionDate { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
