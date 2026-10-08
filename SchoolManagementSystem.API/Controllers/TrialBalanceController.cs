using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.School.TrialBalances.Queries;
using System.Threading.Tasks;

namespace SchoolManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrialBalanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrialBalanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetTrialBalanceQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
