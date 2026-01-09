using SchoolManagementSystem.Application.School.Students.Models;

namespace SchoolManagementSystem.Application.School.Students.Commands;
public class InsertStudentInfoCommand : IHttpRequest
{
    public StudentInfoRequest? StudentInfo { get; set; }

}

