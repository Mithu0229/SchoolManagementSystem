using SchoolManagementSystem.API.Helpers;
using SchoolManagementSystem.Application.School.Institutes.Commands;
using SchoolManagementSystem.Application.School.Institutes.Models;
using SchoolManagementSystem.Application.School.Institutes.Queries;

namespace SchoolManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstituteController : ProtectedBaseController
{
    private readonly IWebHostEnvironment _env;

    public InstituteController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("get-institute-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstituteResponse))]
    public async Task<IResult> GetInstituteList([FromBody] PagedRequest request)
    {
        return await Mediator.Send(new GetInstituteListQuery() { PagedRequest = request });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstituteResponse))]
    public async Task<IResult> Get(Guid id)
    {
        return await Mediator.Send(new GetInstituteByIdQuery(id));
    }

    [HttpPost("save-institute")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstituteResponse))]
    public async Task<IResult> Post([FromForm] InstituteRequest request)
    {
        var logoPath = await LocalImageStorage.SaveAsync(_env, request.Logo, "institutes");
        if (logoPath is null && request.Logo is not null)
        {
            return Result.Fail(StatusCodes.Status400BadRequest, "Only jpg, jpeg, png, gif, or webp logo files are allowed.");
        }

        request.LogoPath = logoPath ?? request.LogoPath;
        InsertInstituteCommand cmd = new InsertInstituteCommand() { Institute = request };
        return await Mediator.Send(cmd);
    }

    [HttpPut("update-institute")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstituteResponse))]
    public async Task<IResult> Put([FromForm] InstituteRequest request)
    {
        var logoPath = await LocalImageStorage.SaveAsync(_env, request.Logo, "institutes");
        if (logoPath is null && request.Logo is not null)
        {
            return Result.Fail(StatusCodes.Status400BadRequest, "Only jpg, jpeg, png, gif, or webp logo files are allowed.");
        }

        request.LogoPath = logoPath ?? request.LogoPath;
        UpdateInstituteCommand cmd = new UpdateInstituteCommand() { Institute = request };
        return await Mediator.Send(cmd);
    }

    [HttpDelete("delete-institute/{id}")]
    public async Task<IResult> DeleteInstitute(Guid id)
    {
        DeleteInstituteCommand cmd = new DeleteInstituteCommand(id);
        return await Mediator.Send(cmd);
    }
}
