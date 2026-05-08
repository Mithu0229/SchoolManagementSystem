using SchoolManagementSystem.Application.School.StudentGroups.Models;

namespace SchoolManagementSystem.Application.School.StudentGroups.Commands;

public class InsertStudentGroupCommand : IHttpRequest
{
    public StudentGroupRequest StudentGroup { get; set; }
}
