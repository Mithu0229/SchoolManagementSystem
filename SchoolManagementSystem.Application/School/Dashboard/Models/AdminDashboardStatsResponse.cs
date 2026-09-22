namespace SchoolManagementSystem.Application.School.Dashboard.Models;

public class AdminDashboardStatsResponse
{
    public int TotalStudents { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalAdmissions { get; set; }
    public int TotalTeachers { get; set; }
    public decimal TotalDue { get; set; }
    public List<MonthlyRevenueDto> RevenueOverview { get; set; } = new();
    public List<ClassDemographicsDto> StudentDemographics { get; set; } = new();
    public List<RecentAdmissionDto> RecentAdmissions { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Expenses { get; set; }
}

public class ClassDemographicsDto
{
    public string ClassName { get; set; } = string.Empty;
    public int StudentCount { get; set; }
}

public class RecentAdmissionDto
{
    public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string RollNo { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
