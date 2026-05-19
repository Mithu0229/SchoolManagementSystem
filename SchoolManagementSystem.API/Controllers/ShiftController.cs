using SchoolManagementSystem.Application.School.Shifts.Commands;
using SchoolManagementSystem.Application.School.Shifts.Models;
using SchoolManagementSystem.Application.School.Shifts.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class ShiftController : ProtectedBaseController
{
    [HttpPost("get-shift-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShiftResponse))]
    public async Task<IResult> GetShiftList([FromBody] PagedRequest request) => await Mediator.Send(new GetShiftListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShiftResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetShiftByIdQuery(id));

    [HttpPost("save-shift")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShiftResponse))]
    public async Task<IResult> Post([FromBody] ShiftRequest request) => await Mediator.Send(new InsertShiftCommand() { Shift = request });

    [HttpPut("update-shift")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShiftResponse))]
    public async Task<IResult> Put([FromBody] ShiftRequest request) => await Mediator.Send(new UpdateShiftCommand() { Shift = request });

    [HttpDelete("delete-shift/{id}")]
    public async Task<IResult> DeleteShift(Guid id) => await Mediator.Send(new DeleteShiftCommand(id));

    [HttpGet("get-shift-dropdown")]
    public async Task<IResult> GetShiftDropdown() => await Mediator.Send(new GetShiftDropdownQuery());
}
