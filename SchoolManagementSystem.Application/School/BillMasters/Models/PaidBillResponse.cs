namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class PaidBillResponse
{
    public Guid Id { get; set; }
    public Guid AdmissionId { get; set; }
    public string? StudentName { get; set; }
    public string? StdCID { get; set; }
    public string? TransactionType { get; set; }
    public int BillMonth { get; set; }
    public string? MonthName { get; set; }
    public int BillYear { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
