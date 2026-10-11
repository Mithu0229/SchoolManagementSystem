using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.School.Accounting.Models;
using SchoolManagementSystem.Application.School.Accounting.Queries;

namespace SchoolManagementSystem.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountingController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("cashbook")]
    public async Task<ActionResult<List<CashBookReportDto>>> GetCashBookReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var result = await _mediator.Send(new GetCashBookReportQuery { StartDate = startDate, EndDate = endDate });
        return Ok(result);
    }

    [HttpGet("bankbook")]
    public async Task<ActionResult<List<BankBookReportDto>>> GetBankBookReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var result = await _mediator.Send(new GetBankBookReportQuery { StartDate = startDate, EndDate = endDate });
        return Ok(result);
    }

    [HttpGet("income-statement")]
    public async Task<ActionResult<IncomeStatementDto>> GetIncomeStatement([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        var result = await _mediator.Send(new GetIncomeStatementQuery { FromDate = fromDate, ToDate = toDate });
        return Ok(result);
    }

    [HttpGet("balance-sheet")]
    public async Task<ActionResult<BalanceSheetDto>> GetBalanceSheet([FromQuery] DateTime asOfDate)
    {
        var result = await _mediator.Send(new GetBalanceSheetQuery { AsOfDate = asOfDate });
        return Ok(result);
    }
}
