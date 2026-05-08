using SchoolManagementSystem.Application.School.StudentGroups.Models;

namespace SchoolManagementSystem.Application.School.StudentGroups.Commands;

public class UpdateStudentGroupCommand : IHttpRequest
{
    public StudentGroupRequest StudentGroup { get; set; }
}
