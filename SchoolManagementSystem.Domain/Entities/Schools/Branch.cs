namespace SchoolManagementSystem.Domain.Entities;

public class Branch : AuditableEntity
{
    public required string BranchName { get; set; }
    public string? BranchAddress { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactNumber { get; set; }
    public string? HomeThemeImagePath { get; set; }

    public required Institute Institute { get; set; }
}
