namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class MultiMonthBillCollectionResponse
{
    public string VoucherNo { get; set; } = string.Empty;
    public int PaidBillsCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string? StCID { get; set; }
    public string? Message { get; set; }
    public string? TrxId { get; set; }
}
