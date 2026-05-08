using SchoolManagementSystem.Application.School.Institutes.Commands;
using SchoolManagementSystem.Application.School.Institutes.Models;
using SchoolManagementSystem.Application.School.Institutes.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class InstituteController : ProtectedBaseController
{
    [HttpPost("get-institute-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstituteResponse))]
    public async Task<IResult> GetInstituteList([FromBody] PagedRequest request)
    {
        return await Mediator.Send(new GetInstituteListQuery() { PagedRequest = request });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstituteResponse))]
    public async Task<IResult> Get(Guid id)
    {
        return await Mediator.Send(new GetInstituteByIdQuery(id));
    }

    [HttpPost("save-institute")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstituteResponse))]
    public async Task<IResult> Post([FromBody] InstituteRequest request)
    {
        InsertInstituteCommand cmd = new InsertInstituteCommand() { Institute = request };
        return await Mediator.Send(cmd);
    }

    [HttpPut("update-institute")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstituteResponse))]
    public async Task<IResult> Put([FromBody] InstituteRequest request)
    {
        UpdateInstituteCommand cmd = new UpdateInstituteCommand() { Institute = request };
        return await Mediator.Send(cmd);
    }

    [HttpDelete("delete-institute/{id}")]
    public async Task<IResult> DeleteInstitute(Guid id)
    {
        DeleteInstituteCommand cmd = new DeleteInstituteCommand(id);
        return await Mediator.Send(cmd);
    }
}
