using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Commands;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class BillMasterController : ProtectedBaseController
{
    [HttpPost("get-bill-master-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillMasterResponse))]
    public async Task<IResult> GetBillMasterList([FromBody] PagedRequest request) => await Mediator.Send(new GetBillMasterListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillMasterResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetBillMasterByIdQuery(id));

    [HttpPost("process-bill")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillMasterResponse))]
    public async Task<IResult> ProcessBill([FromBody] ProcessBillRequest request) => await Mediator.Send(new InsertBillMasterCommand() { ProcessBill = request });

    [HttpPut("update-bill-master")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillMasterResponse))]
    public async Task<IResult> Put([FromBody] BillMasterRequest request) => await Mediator.Send(new UpdateBillMasterCommand() { BillMaster = request });

    [HttpDelete("delete-bill-master/{id}")]
    public async Task<IResult> DeleteBillMaster(Guid id) => await Mediator.Send(new DeleteBillMasterCommand(id));

    [HttpGet("get-money-receipt/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MoneyReceiptResponse))]
    public async Task<IResult> GetMoneyReceipt(Guid id) => await Mediator.Send(new GetMoneyReceiptQuery(id));

    [HttpGet("get-bill-master-dropdown")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DropdownModel>))]
    public async Task<IResult> GetBillMasterDropdown() => await Mediator.Send(new GetBillMasterDropdownQuery());

    [HttpGet("get-fees-due-by-student/{studentId}/{month}/{year}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FeesDueResponse>))]
    public async Task<IResult> GetFeesDueByStudentId(Guid studentId, int month, int year) => await Mediator.Send(new GetFeesDueByStudentIdQuery(studentId, month, year));
}
