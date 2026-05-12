namespace SchoolManagementSystem.Application.School.Branches.Models;

public class BranchRequest
{
    public Guid Id { get; set; }
    public string BranchName { get; set; }
    public string? BranchAddress { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactNumber { get; set; }
    public IFormFile? HomeThemeImage { get; set; }
    public string? HomeThemeImagePath { get; set; }
    public Guid InstituteId { get; set; }
    public bool IsActive { get; set; }
}
