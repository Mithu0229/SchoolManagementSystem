using SchoolManagementSystem.Application.School.StudentFeeLedgers.Commands;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Models;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class StudentFeeLedgerController : ProtectedBaseController
{
    [HttpPost("get-student-fee-ledger-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentFeeLedgerResponse))]
    public async Task<IResult> GetStudentFeeLedgerList([FromBody] PagedRequest request) => await Mediator.Send(new GetStudentFeeLedgerListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentFeeLedgerResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetStudentFeeLedgerByIdQuery(id));

    [HttpPost("save-student-fee-ledger")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentFeeLedgerResponse))]
    public async Task<IResult> Post([FromBody] StudentFeeLedgerRequest request) => await Mediator.Send(new InsertStudentFeeLedgerCommand() { StudentFeeLedger = request });

    [HttpPut("update-student-fee-ledger")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentFeeLedgerResponse))]
    public async Task<IResult> Put([FromBody] StudentFeeLedgerRequest request) => await Mediator.Send(new UpdateStudentFeeLedgerCommand() { StudentFeeLedger = request });

    [HttpDelete("delete-student-fee-ledger/{id}")]
    public async Task<IResult> DeleteStudentFeeLedger(Guid id) => await Mediator.Send(new DeleteStudentFeeLedgerCommand(id));
}
