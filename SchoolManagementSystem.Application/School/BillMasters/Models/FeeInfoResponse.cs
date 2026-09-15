namespace SchoolManagementSystem.Application.School.BillMasters.Models;

public class FeeInfoResponse
{
    public List<FeeSummary> Summary { get; set; } = new();
    public List<FeeHistory> History { get; set; } = new();
}

public class FeeSummary
{
    public string Title { get; set; }
    public string Amount { get; set; }
}

public class FeeHistory
{
    public string Month { get; set; }
    public string Amount { get; set; }
    public string Status { get; set; }
}
