using SchoolManagementSystem.API.Helpers;
using SchoolManagementSystem.Application.School.Teachers.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class TeacherController : ProtectedBaseController
{
    [HttpGet("get-teacher-dropdown")]
    public async Task<IResult> GetTeacherDropdown() => await Mediator.Send(new GetTeacherDropdownQuery());
}
