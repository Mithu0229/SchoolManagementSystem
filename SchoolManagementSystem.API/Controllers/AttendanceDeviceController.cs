using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AttendanceDevices.Commands;
using SchoolManagementSystem.Application.School.AttendanceDevices.Models;
using SchoolManagementSystem.Application.School.AttendanceDevices.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class AttendanceDeviceController : ProtectedBaseController
{
    [HttpPost("get-attendance-device-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDeviceResponse))]
    public async Task<IResult> GetAttendanceDeviceList([FromBody] PagedRequest request) => await Mediator.Send(new GetAttendanceDeviceListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDeviceResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetAttendanceDeviceByIdQuery() { Id = id });

    [HttpPost("save-attendance-device")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDeviceResponse))]
    public async Task<IResult> Post([FromBody] AttendanceDeviceRequest request) => await Mediator.Send(new InsertAttendanceDeviceCommand() { AttendanceDevice = request });

    [HttpPut("update-attendance-device")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDeviceResponse))]
    public async Task<IResult> Put([FromBody] AttendanceDeviceRequest request) => await Mediator.Send(new UpdateAttendanceDeviceCommand() { AttendanceDevice = request });

    [HttpDelete("delete-attendance-device/{id}")]
    public async Task<IResult> DeleteAttendanceDevice(Guid id) => await Mediator.Send(new DeleteAttendanceDeviceCommand() { Id = id });

    [HttpGet("get-attendance-device-dropdown")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DropdownModel>))]
    public async Task<IResult> GetAttendanceDeviceDropdown() => await Mediator.Send(new GetAttendanceDeviceDropdownQuery());
}
