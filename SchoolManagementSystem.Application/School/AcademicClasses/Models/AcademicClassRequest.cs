namespace SchoolManagementSystem.Application.School.AcademicClasses.Models;

public class AcademicClassRequest
{
    public Guid Id { get; set; }
    public string ClassName { get; set; }
    public string? ClassDetails { get; set; }
    public bool IsActive { get; set; }
}
