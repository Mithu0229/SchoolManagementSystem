using SchoolManagementSystem.Application.School.AcademicClasses.Commands;
using SchoolManagementSystem.Application.School.AcademicClasses.Models;
using SchoolManagementSystem.Application.School.AcademicClasses.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class AcademicClassController : ProtectedBaseController
{
    [HttpPost("get-academic-class-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AcademicClassResponse))]
    public async Task<IResult> GetAcademicClassList([FromBody] PagedRequest request) => await Mediator.Send(new GetAcademicClassListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AcademicClassResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetAcademicClassByIdQuery(id));

    [HttpPost("save-academic-class")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AcademicClassResponse))]
    public async Task<IResult> Post([FromBody] AcademicClassRequest request) => await Mediator.Send(new InsertAcademicClassCommand() { AcademicClass = request });

    [HttpPut("update-academic-class")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AcademicClassResponse))]
    public async Task<IResult> Put([FromBody] AcademicClassRequest request) => await Mediator.Send(new UpdateAcademicClassCommand() { AcademicClass = request });

    [HttpDelete("delete-academic-class/{id}")]
    public async Task<IResult> DeleteAcademicClass(Guid id) => await Mediator.Send(new DeleteAcademicClassCommand(id));

    [HttpGet("get-academic-class-dropdown")]
    public async Task<IResult> GetAcademicClassDropdown() => await Mediator.Send(new GetAcademicClassDropdownQuery());
}
