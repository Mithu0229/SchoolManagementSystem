namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class PaidFeeResponse
{
    public Guid Id { get; set; }
    public string Date { get; set; }
    public string Amount { get; set; }
    public string Slip { get; set; }
}
