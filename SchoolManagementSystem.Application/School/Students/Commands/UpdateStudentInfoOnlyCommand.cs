using SchoolManagementSystem.Application.School.Students.Models;

namespace SchoolManagementSystem.Application.School.Students.Commands;

public class UpdateStudentInfoOnlyCommand : IHttpRequest
{
    public StudentInfoUpdateRequest? StudentInfo { get; set; }
}
