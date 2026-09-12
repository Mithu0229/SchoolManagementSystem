namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class MultiMonthMoneyReceiptResponse
{
    public string ReceiptNo { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string AdmissionNo { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    
    public List<MultiMonthReceiptDetail> Details { get; set; } = new List<MultiMonthReceiptDetail>();
    
    public decimal TotalAmount { get; set; }
    public decimal CollectionAmount { get; set; }
    public decimal DueAmount { get; set; }
    
    public string PaymentMethod { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
}

public class MultiMonthReceiptDetail
{
    public string Month { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public decimal Collection { get; set; }
    public decimal Due { get; set; }
}
