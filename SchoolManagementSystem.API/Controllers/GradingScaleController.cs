using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.GradingScales.Commands;
using SchoolManagementSystem.Application.School.GradingScales.Models;
using SchoolManagementSystem.Application.School.GradingScales.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class GradingScaleController : PublicBaseController
{
    [HttpPost("get-grading-scale-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GradingScaleResponse>))]
    public async Task<IResult> GetGradingScaleList([FromBody] PagedRequest request) =>
        await Mediator.Send(new GetGradingScaleListQuery { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GradingScaleResponse))]
    public async Task<IResult> Get(Guid id) =>
        await Mediator.Send(new GetGradingScaleByIdQuery(id));

    [HttpGet("get-default")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GradingScaleResponse))]
    public async Task<IResult> GetDefault() =>
        await Mediator.Send(new GetDefaultGradingScaleQuery());

    [HttpPost("save-grading-scale")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GradingScaleResponse))]
    public async Task<IResult> Post([FromBody] GradingScaleRequest request) =>
        await Mediator.Send(new SaveGradingScaleCommand { GradingScale = request });

    [HttpDelete("delete-grading-scale/{id}")]
    public async Task<IResult> Delete(Guid id) =>
        await Mediator.Send(new DeleteGradingScaleCommand(id));
}
