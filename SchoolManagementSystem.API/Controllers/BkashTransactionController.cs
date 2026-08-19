using SchoolManagementSystem.Application.School.BkashTransactions.Commands;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.School.BkashTransactions.Queries;
using SchoolManagementSystem.Application.School.PayBills.Models;

namespace SchoolManagementSystem.API.Controllers;

public class BkashTransactionController : ProtectedBaseController
{
    [HttpPost("get-bkash-transaction-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BkashTransactionResponse))]
    public async Task<IResult> GetBkashTransactionList([FromBody] PagedRequest request) => await Mediator.Send(new GetBkashTransactionListQuery() { PagedRequest = request });

    //[HttpGet("{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BkashTransactionResponse))]
    //public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetBkashTransactionByIdQuery() { Id = id });

     
    //[HttpPut("update-bkash-transaction/{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BkashTransactionResponse))]
    //public async Task<IResult> Put(Guid id, [FromBody] BkashTransactionRequest request) => await Mediator.Send(new UpdateBkashTransactionCommand() { Id = id, BkashTransaction = request });

    //[HttpDelete("delete-bkash-transaction/{id}")]
    //public async Task<IResult> DeleteBkashTransaction(Guid id) => await Mediator.Send(new DeleteBkashTransactionCommand() { Id = id });

   
    [HttpPost("check-bill")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CheckBillResponse))]
    public async Task<IResult> CheckBill([FromBody] CheckBillRequest request) => await Mediator.Send(new CheckBillCommand() { Request = request });

    [HttpPost("pay-bill")]//paybill
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BkashTransactionResponse))]
    public async Task<IResult> Post([FromBody] BkashTransactionRequest request) => await Mediator.Send(new InsertBkashTransactionCommand() { BkashTransaction = request });

    [HttpPost("tsq")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TSQResponse))]
    public async Task<IResult> TSQ([FromBody] TSQRequest request) => await Mediator.Send(new TSQQueryCommand() { Request = request });



}
