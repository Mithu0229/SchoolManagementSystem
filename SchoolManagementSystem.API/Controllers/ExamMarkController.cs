using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.ExamMarks.Commands;
using SchoolManagementSystem.Application.School.ExamMarks.Models;
using SchoolManagementSystem.Application.School.ExamMarks.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class ExamMarkController : PublicBaseController
{
    [HttpGet("get-exam-components")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ExamComponentDto>))]
    public async Task<IResult> GetExamComponents([FromQuery] Guid? academicSessionId) =>
        await Mediator.Send(new GetExamComponentsQuery { AcademicSessionId = academicSessionId });

    [HttpPost("save-exam-component")]
    public async Task<IResult> SaveExamComponent([FromBody] ExamComponentDto component) =>
        await Mediator.Send(new SaveExamComponentCommand { Component = component });

    [HttpPost("get-student-marksheet")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentMarksheetResponse))]
    public async Task<IResult> GetStudentMarksheet([FromBody] GetStudentMarksheetQuery query) =>
        await Mediator.Send(query);

    [HttpPost("save-student-marks")]
    public async Task<IResult> SaveStudentMarks([FromBody] SaveStudentMarksRequest request) =>
        await Mediator.Send(new SaveStudentMarksCommand { Request = request });

    [HttpPost("get-batch-marks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BatchStudentMarkItem>))]
    public async Task<IResult> GetBatchMarks([FromBody] GetBatchMarksQuery query) =>
        await Mediator.Send(query);

    [HttpPost("save-batch-marks")]
    public async Task<IResult> SaveBatchMarks([FromBody] SaveBatchMarksRequest request) =>
        await Mediator.Send(new SaveBatchMarksCommand { Request = request });

    [HttpGet("get-students-by-class")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentDropdownItem>))]
    public async Task<IResult> GetStudentsByClass([FromQuery] Guid sessionId, [FromQuery] Guid classId, [FromQuery] Guid? sectionId) =>
        await Mediator.Send(new GetStudentsByClassQuery { AcademicSessionId = sessionId, ClassId = classId, SectionId = sectionId });
}
