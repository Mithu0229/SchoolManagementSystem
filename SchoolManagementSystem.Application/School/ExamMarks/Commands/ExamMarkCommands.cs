using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.ExamMarks.Models;

namespace SchoolManagementSystem.Application.School.ExamMarks.Commands;

public class SaveStudentMarksCommand : IHttpRequest
{
    public required SaveStudentMarksRequest Request { get; set; }
}

public class SaveBatchMarksCommand : IHttpRequest
{
    public required SaveBatchMarksRequest Request { get; set; }
}

public class SaveExamComponentCommand : IHttpRequest
{
    public required ExamComponentDto Component { get; set; }
}
