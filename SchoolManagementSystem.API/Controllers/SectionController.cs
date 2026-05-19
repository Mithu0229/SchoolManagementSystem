using SchoolManagementSystem.Application.School.Sections.Commands;
using SchoolManagementSystem.Application.School.Sections.Models;
using SchoolManagementSystem.Application.School.Sections.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class SectionController : ProtectedBaseController
{
    [HttpPost("get-section-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionResponse))]
    public async Task<IResult> GetSectionList([FromBody] PagedRequest request) => await Mediator.Send(new GetSectionListQuery() { PagedRequest = request });

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionResponse))]
    public async Task<IResult> Get(Guid id) => await Mediator.Send(new GetSectionByIdQuery(id));

    [HttpPost("save-section")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionResponse))]
    public async Task<IResult> Post([FromBody] SectionRequest request) => await Mediator.Send(new InsertSectionCommand() { Section = request });

    [HttpPut("update-section")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionResponse))]
    public async Task<IResult> Put([FromBody] SectionRequest request) => await Mediator.Send(new UpdateSectionCommand() { Section = request });

    [HttpDelete("delete-section/{id}")]
    public async Task<IResult> DeleteSection(Guid id) => await Mediator.Send(new DeleteSectionCommand(id));

    [HttpGet("get-section-dropdown")]
    public async Task<IResult> GetSectionDropdown() => await Mediator.Send(new GetSectionDropdownQuery());
}
