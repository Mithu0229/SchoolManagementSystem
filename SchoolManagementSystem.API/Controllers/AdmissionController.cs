using SchoolManagementSystem.Application.School.Admissions.Commands;
using SchoolManagementSystem.Application.School.Admissions.Models;
using SchoolManagementSystem.Application.School.Admissions.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class AdmissionController : ProtectedBaseController
{
    [HttpPost("get-admission-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdmissionResponse))]
    public async Task<IResult> GetAdmissionList([FromBody] PagedRequest request) => await Mediator.Send(new GetAdmissionListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdmissionResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetAdmissionByIdQuery(id));

    [HttpPost("save-admission")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdmissionResponse))]
    public async Task<IResult> Post([FromBody] AdmissionRequest request) => await Mediator.Send(new InsertAdmissionCommand() { Admission = request });

    [HttpPut("update-admission")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdmissionResponse))]
    public async Task<IResult> Put([FromBody] AdmissionRequest request) => await Mediator.Send(new UpdateAdmissionCommand() { Admission = request });

    [HttpDelete("delete-admission/{id}")]
    public async Task<IResult> DeleteAdmission(Guid id) => await Mediator.Send(new DeleteAdmissionCommand(id));
}
