namespace SchoolManagementSystem.Application.School.FeeHeads.Models;

public class FeeHeadResponse
{
    public Guid Id { get; set; }
    public string FeeHeadName { get; set; }
    public bool IsMonthly { get; set; }
    public bool IsActive { get; set; }
}
