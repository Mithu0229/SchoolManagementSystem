using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Subjects.Models;

namespace SchoolManagementSystem.Application.School.Subjects.Commands;

public class InsertSubjectCommand : IHttpRequest
{
    public required SubjectRequest Subject { get; set; }
}

public class UpdateSubjectCommand : IHttpRequest
{
    public required SubjectRequest Subject { get; set; }
}

public class DeleteSubjectCommand(Guid id) : IHttpRequest
{
    public Guid Id { get; set; } = id;
}

public class AssignClassSubjectsCommand : IHttpRequest
{
    public required ClassSubjectDto ClassSubject { get; set; }
}
