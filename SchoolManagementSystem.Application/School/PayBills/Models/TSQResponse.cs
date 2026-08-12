namespace SchoolManagementSystem.Application.School.PayBills.Models;

public class TSQResponse
{
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMsg { get; set; } = string.Empty;
    public string? TotalAmount { get; set; }
    public string? TrxId { get; set; }
    public string? MiddlewarePayTime { get; set; }
    public string? RefNumber { get; set; }
    public string? CustomMessage { get; set; }
    public string? AmountBreakdown { get; set; }
}
