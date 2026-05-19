using SchoolManagementSystem.API.Helpers;
using SchoolManagementSystem.Application.School.Branches.Commands;
using SchoolManagementSystem.Application.School.Branches.Models;
using SchoolManagementSystem.Application.School.Branches.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class BranchController : ProtectedBaseController
{
    private readonly IWebHostEnvironment _env;

    public BranchController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("get-branch-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BranchResponse))]
    public async Task<IResult> GetBranchList([FromBody] PagedRequest request)
    {
        return await Mediator.Send(new GetBranchListQuery() { PagedRequest = request });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BranchResponse))]
    public async Task<IResult> Get(Guid id)
    {
        return await Mediator.Send(new GetBranchByIdQuery(id));
    }

    [HttpPost("save-branch")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BranchResponse))]
    public async Task<IResult> Post([FromForm] BranchRequest request)
    {
        var imagePath = await LocalImageStorage.SaveAsync(_env, request.HomeThemeImage, "branches");
        if (imagePath is null && request.HomeThemeImage is not null)
        {
            return Result.Fail(StatusCodes.Status400BadRequest, "Only jpg, jpeg, png, gif, or webp image files are allowed.");
        }

        request.HomeThemeImagePath = imagePath ?? request.HomeThemeImagePath;
        InsertBranchCommand cmd = new InsertBranchCommand() { Branch = request };
        return await Mediator.Send(cmd);
    }

    [HttpPut("update-branch")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BranchResponse))]
    public async Task<IResult> Put([FromForm] BranchRequest request)
    {
        var imagePath = await LocalImageStorage.SaveAsync(_env, request.HomeThemeImage, "branches");
        if (imagePath is null && request.HomeThemeImage is not null)
        {
            return Result.Fail(StatusCodes.Status400BadRequest, "Only jpg, jpeg, png, gif, or webp image files are allowed.");
        }

        request.HomeThemeImagePath = imagePath ?? request.HomeThemeImagePath;
        UpdateBranchCommand cmd = new UpdateBranchCommand() { Branch = request };
        return await Mediator.Send(cmd);
    }

    [HttpDelete("delete-branch/{id}")]
    public async Task<IResult> DeleteBranch(Guid id)
    {
        DeleteBranchCommand cmd = new DeleteBranchCommand(id);
        return await Mediator.Send(cmd);
    }

    [HttpGet("get-branch-dropdown")]
    public async Task<IResult> GetBranchDropdown() => await Mediator.Send(new GetBranchDropdownQuery());
}
