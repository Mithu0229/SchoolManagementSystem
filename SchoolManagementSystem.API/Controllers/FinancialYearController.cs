using SchoolManagementSystem.Application.School.FinancialYears.Commands;
using SchoolManagementSystem.Application.School.FinancialYears.Models;
using SchoolManagementSystem.Application.School.FinancialYears.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class FinancialYearController : ProtectedBaseController
{
    [HttpPost("get-financial-year-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinancialYearResponse))]
    public async Task<IResult> GetFinancialYearList([FromBody] PagedRequest request)
    {
        return await Mediator.Send(new GetFinancialYearListQuery() { PagedRequest = request });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinancialYearResponse))]
    public async Task<IResult> Get(Guid id)
    {
        return await Mediator.Send(new GetFinancialYearByIdQuery(id));
    }

    [HttpPost("save-financial-year")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinancialYearResponse))]
    public async Task<IResult> Post([FromBody] FinancialYearRequest request)
    {
        InsertFinancialYearCommand cmd = new InsertFinancialYearCommand() { FinancialYear = request };
        return await Mediator.Send(cmd);
    }

    [HttpPut("update-financial-year")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinancialYearResponse))]
    public async Task<IResult> Put([FromBody] FinancialYearRequest request)
    {
        UpdateFinancialYearCommand cmd = new UpdateFinancialYearCommand() { FinancialYear = request };
        return await Mediator.Send(cmd);
    }

    [HttpDelete("delete-financial-year/{id}")]
    public async Task<IResult> DeleteFinancialYear(Guid id)
    {
        DeleteFinancialYearCommand cmd = new DeleteFinancialYearCommand(id);
        return await Mediator.Send(cmd);
    }
}
