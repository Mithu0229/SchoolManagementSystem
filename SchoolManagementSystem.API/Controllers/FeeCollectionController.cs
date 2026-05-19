using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.FeeCollections.Commands;
using SchoolManagementSystem.Application.School.FeeCollections.Models;
using SchoolManagementSystem.Application.School.FeeCollections.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class FeeCollectionController : ProtectedBaseController
{
    [HttpPost("get-fee-collection-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeCollectionResponse))]
    public async Task<IResult> GetFeeCollectionList([FromBody] PagedRequest request) => await Mediator.Send(new GetFeeCollectionListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeCollectionResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetFeeCollectionByIdQuery(id));

    [HttpPost("save-fee-collection")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeCollectionResponse))]
    public async Task<IResult> Post([FromBody] FeeCollectionRequest request) => await Mediator.Send(new InsertFeeCollectionCommand() { FeeCollection = request });

    [HttpPut("update-fee-collection")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeCollectionResponse))]
    public async Task<IResult> Put([FromBody] FeeCollectionRequest request) => await Mediator.Send(new UpdateFeeCollectionCommand() { FeeCollection = request });

    [HttpDelete("delete-fee-collection/{id}")]
    public async Task<IResult> DeleteFeeCollection(Guid id) => await Mediator.Send(new DeleteFeeCollectionCommand(id));

    [HttpGet("get-fee-collection-dropdown")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DropdownModel>))]
    public async Task<IResult> GetFeeCollectionDropdown() => await Mediator.Send(new GetFeeCollectionDropdownQuery());
}
