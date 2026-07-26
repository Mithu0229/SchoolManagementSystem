using SchoolManagementSystem.Application.School.SchoolStudents.Commands;
using SchoolManagementSystem.Application.School.SchoolStudents.Models;
using SchoolManagementSystem.Application.School.SchoolStudents.Queries;
using SchoolManagementSystem.Application.School.SMSHistories.Queries;
using StudentDropdownQuery = SchoolManagementSystem.Application.School.Students.Queries.GetStudentDropdownQuery;

namespace SchoolManagementSystem.API.Controllers;

public class StudentController : ProtectedBaseController
{
    [HttpPost("get-student-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponse))]
    public async Task<IResult> GetStudentList([FromBody] PagedRequest request) => await Mediator.Send(new GetStudentListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetStudentByIdQuery(id));

    [HttpPost("save-student")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponse))]
    public async Task<IResult> Post([FromBody] StudentRequest request) => await Mediator.Send(new InsertStudentCommand() { Student = request });

    [HttpPut("update-student")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponse))]
    public async Task<IResult> Put([FromBody] StudentRequest request) => await Mediator.Send(new UpdateStudentCommand() { Student = request });

    [HttpDelete("delete-student/{id}")]
    public async Task<IResult> DeleteStudent(Guid id) => await Mediator.Send(new DeleteStudentCommand(id));

    [HttpGet("get-student-dropdown")]
    public async Task<IResult> GetStudentDropdown() => await Mediator.Send(new StudentDropdownQuery());

    [HttpGet("get-sms-history/{studentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SMSHistoryResponse>))]
    public async Task<IResult> GetSMSHistory(Guid studentId) => await Mediator.Send(new GetSMSHistoryByStudentIdQuery { StudentId = studentId });
}
