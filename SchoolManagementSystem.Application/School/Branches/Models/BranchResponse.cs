namespace SchoolManagementSystem.Application.School.Branches.Models;

public class BranchResponse
{
    public Guid Id { get; set; }
    public string BranchName { get; set; }
    public string? BranchAddress { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactNumber { get; set; }
    public string? HomeThemeImagePath { get; set; }
    public Guid InstituteId { get; set; }
    public string? InstituteName { get; set; }
    public bool IsActive { get; set; }
}
