namespace SchoolManagementSystem.Application.School.Sections.Models;

public class SectionRequest
{
    public Guid Id { get; set; }
    public string SectionName { get; set; }
    public string? Remarks { get; set; }
    public bool IsActive { get; set; }
}
