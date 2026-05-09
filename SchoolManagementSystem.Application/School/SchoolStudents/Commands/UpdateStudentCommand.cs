using SchoolManagementSystem.Application.School.SchoolStudents.Models;

namespace SchoolManagementSystem.Application.School.SchoolStudents.Commands;

public class UpdateStudentCommand : IHttpRequest
{
    public StudentRequest Student { get; set; }
}
