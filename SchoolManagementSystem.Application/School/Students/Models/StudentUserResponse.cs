namespace SchoolManagementSystem.Application.School.Students.Models;

public class StudentUserResponse
{
    public Guid StudentId { get; set; }
    public string StdCID { get; set; }
    public string FullName { get; set; }
    public string StudentPhone { get; set; }
    public string StudentEmail { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
}
