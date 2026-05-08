namespace SchoolManagementSystem.Application.School.Institutes.Models;

public class InstituteResponse
{
    public Guid Id { get; set; }
    public string InstituteName { get; set; }
    public string? Address { get; set; }
    public string? ContactNo { get; set; }
    public string? Email { get; set; }
    public string? LogoPath { get; set; }
    public bool IsActive { get; set; }
}
