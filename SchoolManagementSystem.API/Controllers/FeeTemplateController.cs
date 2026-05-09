using SchoolManagementSystem.Application.School.FeeTemplates.Commands;
using SchoolManagementSystem.Application.School.FeeTemplates.Models;
using SchoolManagementSystem.Application.School.FeeTemplates.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class FeeTemplateController : ProtectedBaseController
{
    [HttpPost("get-fee-template-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeTemplateResponse))]
    public async Task<IResult> GetFeeTemplateList([FromBody] PagedRequest request) => await Mediator.Send(new GetFeeTemplateListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeTemplateResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetFeeTemplateByIdQuery(id));

    [HttpPost("save-fee-template")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeTemplateResponse))]
    public async Task<IResult> Post([FromBody] FeeTemplateRequest request) => await Mediator.Send(new InsertFeeTemplateCommand() { FeeTemplate = request });

    [HttpPut("update-fee-template")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeeTemplateResponse))]
    public async Task<IResult> Put([FromBody] FeeTemplateRequest request) => await Mediator.Send(new UpdateFeeTemplateCommand() { FeeTemplate = request });

    [HttpDelete("delete-fee-template/{id}")]
    public async Task<IResult> DeleteFeeTemplate(Guid id) => await Mediator.Send(new DeleteFeeTemplateCommand(id));
}
