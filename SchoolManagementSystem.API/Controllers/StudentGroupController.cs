using SchoolManagementSystem.Application.School.StudentGroups.Commands;
using SchoolManagementSystem.Application.School.StudentGroups.Models;
using SchoolManagementSystem.Application.School.StudentGroups.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class StudentGroupController : ProtectedBaseController
{
    [HttpPost("get-student-group-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentGroupResponse))]
    public async Task<IResult> GetStudentGroupList([FromBody] PagedRequest request) => await Mediator.Send(new GetStudentGroupListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentGroupResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetStudentGroupByIdQuery(id));

    [HttpPost("save-student-group")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentGroupResponse))]
    public async Task<IResult> Post([FromBody] StudentGroupRequest request) => await Mediator.Send(new InsertStudentGroupCommand() { StudentGroup = request });

    [HttpPut("update-student-group")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentGroupResponse))]
    public async Task<IResult> Put([FromBody] StudentGroupRequest request) => await Mediator.Send(new UpdateStudentGroupCommand() { StudentGroup = request });

    [HttpDelete("delete-student-group/{id}")]
    public async Task<IResult> DeleteStudentGroup(Guid id) => await Mediator.Send(new DeleteStudentGroupCommand(id));

    [HttpGet("get-student-group-dropdown")]
    public async Task<IResult> GetStudentGroupDropdown() => await Mediator.Send(new GetStudentGroupDropdownQuery());
}
