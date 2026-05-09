using SchoolManagementSystem.Application.School.SchoolStudents.Models;

namespace SchoolManagementSystem.Application.School.SchoolStudents.Commands;

public class InsertStudentCommand : IHttpRequest
{
    public StudentRequest Student { get; set; }
}
