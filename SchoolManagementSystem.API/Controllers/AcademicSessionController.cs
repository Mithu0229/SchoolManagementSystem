using SchoolManagementSystem.Application.School.AcademicSessions.Commands;
using SchoolManagementSystem.Application.School.AcademicSessions.Models;
using SchoolManagementSystem.Application.School.AcademicSessions.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class AcademicSessionController : ProtectedBaseController
{
    [HttpPost("get-academic-session-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AcademicSessionResponse))]
    public async Task<IResult> GetAcademicSessionList([FromBody] PagedRequest request)
    {
        return await Mediator.Send(new GetAcademicSessionListQuery() { PagedRequest = request });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AcademicSessionResponse))]
    public async Task<IResult> Get(Guid id)
    {
        return await Mediator.Send(new GetAcademicSessionByIdQuery(id));
    }

    [HttpPost("save-academic-session")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AcademicSessionResponse))]
    public async Task<IResult> Post([FromBody] AcademicSessionRequest request)
    {
        InsertAcademicSessionCommand cmd = new InsertAcademicSessionCommand() { AcademicSession = request };
        return await Mediator.Send(cmd);
    }

    [HttpPut("update-academic-session")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AcademicSessionResponse))]
    public async Task<IResult> Put([FromBody] AcademicSessionRequest request)
    {
        UpdateAcademicSessionCommand cmd = new UpdateAcademicSessionCommand() { AcademicSession = request };
        return await Mediator.Send(cmd);
    }

    [HttpDelete("delete-academic-session/{id}")]
    public async Task<IResult> DeleteAcademicSession(Guid id) => await Mediator.Send(new DeleteAcademicSessionCommand(id));

    [HttpGet("get-academic-session-dropdown")]
    public async Task<IResult> GetAcademicSessionDropdown() => await Mediator.Send(new GetAcademicSessionDropdownQuery());
}
