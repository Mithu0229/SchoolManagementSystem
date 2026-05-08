namespace SchoolManagementSystem.Application.School.StudentGroups.Models;

public class StudentGroupRequest
{
    public Guid Id { get; set; }
    public string GroupName { get; set; }
    public string? GroupDetails { get; set; }
    public bool IsActive { get; set; }
}
