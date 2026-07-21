namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class MoneyReceiptResponse
{
    public Guid BillMasterId { get; set; }
    public string StudentName { get; set; }
    public string StudentID { get; set; }
    public string Phone { get; set; }
    public string ClassName { get; set; }
    public string Date { get; set; }
    public int ManualMR { get; set; }
    public string InvID { get; set; }
    public decimal TotalAmount { get; set; }
    public string Inword { get; set; }
    public List<MoneyReceiptDetailResponse> Details { get; set; } = new List<MoneyReceiptDetailResponse>();
}

public class MoneyReceiptDetailResponse
{
    public int SN { get; set; }
    public string AccountHead { get; set; }
    public string Month { get; set; }
    public string TransID { get; set; }
    public decimal Amount { get; set; }
}
