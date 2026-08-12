namespace SchoolManagementSystem.Application.School.PayBills.Models;

public class CheckBillRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string? AccNo { get; set; }
    public string? MeterNo { get; set; }
    public string? CustomerNo { get; set; }
    public string? BillNo { get; set; }
    public string? RefID { get; set; }

    public string BillMonth { get; set; } = string.Empty;
    public string? Amount { get; set; }

    public string GetReferenceId()
    {
        if (!string.IsNullOrWhiteSpace(CustomerNo)) return CustomerNo.Trim();
        if (!string.IsNullOrWhiteSpace(AccNo)) return AccNo.Trim();
        if (!string.IsNullOrWhiteSpace(BillNo)) return BillNo.Trim();
        if (!string.IsNullOrWhiteSpace(RefID)) return RefID.Trim();
        if (!string.IsNullOrWhiteSpace(MeterNo)) return MeterNo.Trim();
        return string.Empty;
    }
}
