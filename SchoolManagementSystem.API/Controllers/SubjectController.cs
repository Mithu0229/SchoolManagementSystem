using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Subjects.Commands;
using SchoolManagementSystem.Application.School.Subjects.Models;
using SchoolManagementSystem.Application.School.Subjects.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class SubjectController : PublicBaseController
{
    [HttpPost("get-subject-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubjectResponse))]
    public async Task<IResult> GetSubjectList([FromBody] PagedRequest request) =>
        await Mediator.Send(new GetSubjectListQuery { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubjectResponse))]
    public async Task<IResult> Get(Guid id) =>
        await Mediator.Send(new GetSubjectByIdQuery(id));

    [HttpPost("save-subject")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubjectResponse))]
    public async Task<IResult> Post([FromBody] SubjectRequest request) =>
        await Mediator.Send(new InsertSubjectCommand { Subject = request });

    [HttpPut("update-subject")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubjectResponse))]
    public async Task<IResult> Put([FromBody] SubjectRequest request) =>
        await Mediator.Send(new UpdateSubjectCommand { Subject = request });

    [HttpDelete("delete-subject/{id}")]
    public async Task<IResult> DeleteSubject(Guid id) =>
        await Mediator.Send(new DeleteSubjectCommand(id));

    [HttpGet("get-subject-dropdown")]
    public async Task<IResult> GetSubjectDropdown() =>
        await Mediator.Send(new GetSubjectDropdownQuery());

    [HttpGet("get-subjects-by-class/{classId}")]
    public async Task<IResult> GetSubjectsByClass(Guid classId) =>
        await Mediator.Send(new GetSubjectsByClassQuery(classId));

    [HttpPost("assign-class-subjects")]
    public async Task<IResult> AssignClassSubjects([FromBody] ClassSubjectDto request) =>
        await Mediator.Send(new AssignClassSubjectsCommand { ClassSubject = request });
}
