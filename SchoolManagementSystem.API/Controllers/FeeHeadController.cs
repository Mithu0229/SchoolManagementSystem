using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.FeeHeads.Commands;
using SchoolManagementSystem.Application.School.FeeHeads.Models;
using SchoolManagementSystem.Application.School.FeeHeads.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class FeeHeadController : ProtectedBaseController
{
    [HttpPost("get-fee-head-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeHeadResponse))]
    public async Task<IResult> GetFeeHeadList([FromBody] PagedRequest request) => await Mediator.Send(new GetFeeHeadListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeHeadResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetFeeHeadByIdQuery(id));

    [HttpPost("save-fee-head")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeHeadResponse))]
    public async Task<IResult> Post([FromBody] FeeHeadRequest request) => await Mediator.Send(new InsertFeeHeadCommand() { FeeHead = request });

    [HttpPut("update-fee-head")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeHeadResponse))]
    public async Task<IResult> Put([FromBody] FeeHeadRequest request) => await Mediator.Send(new UpdateFeeHeadCommand() { FeeHead = request });

    [HttpDelete("delete-fee-head/{id}")]
    public async Task<IResult> DeleteFeeHead(Guid id) => await Mediator.Send(new DeleteFeeHeadCommand(id));

    [HttpGet("get-fee-head-dropdown")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DropdownModel>))]
    public async Task<IResult> GetFeeHeadDropdown() => await Mediator.Send(new GetFeeHeadDropdownQuery());
}
