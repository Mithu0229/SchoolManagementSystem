namespace SchoolManagementSystem.Application.School.PayBills.Models;

public class CheckBillResponse
{
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMsg { get; set; } = string.Empty;
    public string? ConsumerName { get; set; }
    public string? BillMonth { get; set; }
    public string? BillAmount { get; set; }
    public string? BillDueDate { get; set; }
    public string? QueryTime { get; set; }
    public string? AmountBreakdown { get; set; }
}
