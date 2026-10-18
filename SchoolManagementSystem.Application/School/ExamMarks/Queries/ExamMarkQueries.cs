using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.ExamMarks.Queries;

public class GetExamComponentsQuery : IHttpRequest
{
    public Guid? AcademicSessionId { get; set; }
}

public class GetStudentMarksheetQuery : IHttpRequest
{
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid StudentId { get; set; }
}

public class GetBatchMarksQuery : IHttpRequest
{
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid ExamComponentId { get; set; }
}

public class GetStudentsByClassQuery : IHttpRequest
{
    public Guid AcademicSessionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
}

public class StudentDropdownItem
{
    public Guid StudentId { get; set; }
    public Guid AdmissionId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string RollNo { get; set; } = string.Empty;
}
