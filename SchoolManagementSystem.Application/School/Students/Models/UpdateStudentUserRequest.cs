namespace SchoolManagementSystem.Application.School.Students.Models;

public class UpdateStudentUserRequest
{
    public Guid StudentId { get; set; }
    public bool IsActive { get; set; }
    public string Password { get; set; }
}
