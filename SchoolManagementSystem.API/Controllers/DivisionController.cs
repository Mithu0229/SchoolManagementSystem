using SchoolManagementSystem.Application.GS.Divisions.Commands;
using SchoolManagementSystem.Application.GS.Divisions.Models;
using SchoolManagementSystem.Application.GS.Divisions.Queries;

namespace SchoolManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : ProtectedBaseController
    {
        [HttpPost("get-division-list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DivisionResponse))]
        public async Task<IResult> GetDivisionList([FromBody] PagedRequest request)
        {
            return await Mediator.Send(new GetDivisionListQuery() { PagedRequest = request });

        }

        [HttpPost("save-division")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DivisionResponse))]
        public Task<IResult> Post([FromBody] DivisionRequest request)
        {
            InsertDivisionCommand cmd = new InsertDivisionCommand() { Division = request };
            return Mediator.Send(cmd);
        }

        [HttpPut("update-division")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DivisionResponse))]
        public Task<IResult> Put([FromBody] DivisionRequest request)
        {
            UpdateDivisionCommand cmd = new UpdateDivisionCommand() { Division = request };
            return Mediator.Send(cmd);
        }

        [HttpDelete("delete-division/{id}")]
        public async Task<IResult> DeleteDivision(Guid id)
        {
            DeleteDivisionCommand cmd = new DeleteDivisionCommand(id);
            return await Mediator.Send(cmd);
        }

    }
}
