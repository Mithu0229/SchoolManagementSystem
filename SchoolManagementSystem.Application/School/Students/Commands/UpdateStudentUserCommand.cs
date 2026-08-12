using SchoolManagementSystem.Application.School.Students.Models;

namespace SchoolManagementSystem.Application.School.Students.Commands;

public record UpdateStudentUserCommand : IHttpRequest
{
    public UpdateStudentUserRequest Request { get; set; }
}
