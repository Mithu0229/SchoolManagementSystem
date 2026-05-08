using SchoolManagementSystem.Application.School.Branches.Commands;
using SchoolManagementSystem.Application.School.Branches.Models;
using SchoolManagementSystem.Application.School.Branches.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class BranchController : ProtectedBaseController
{
    [HttpPost("get-branch-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BranchResponse))]
    public async Task<IResult> GetBranchList([FromBody] PagedRequest request)
    {
        return await Mediator.Send(new GetBranchListQuery() { PagedRequest = request });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BranchResponse))]
    public async Task<IResult> Get(Guid id)
    {
        return await Mediator.Send(new GetBranchByIdQuery(id));
    }

    [HttpPost("save-branch")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BranchResponse))]
    public async Task<IResult> Post([FromBody] BranchRequest request)
    {
        InsertBranchCommand cmd = new InsertBranchCommand() { Branch = request };
        return await Mediator.Send(cmd);
    }

    [HttpPut("update-branch")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BranchResponse))]
    public async Task<IResult> Put([FromBody] BranchRequest request)
    {
        UpdateBranchCommand cmd = new UpdateBranchCommand() { Branch = request };
        return await Mediator.Send(cmd);
    }

    [HttpDelete("delete-branch/{id}")]
    public async Task<IResult> DeleteBranch(Guid id)
    {
        DeleteBranchCommand cmd = new DeleteBranchCommand(id);
        return await Mediator.Send(cmd);
    }
}
