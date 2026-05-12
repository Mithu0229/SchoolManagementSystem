using SchoolManagementSystem.Application.School.Institutes.Commands;
using SchoolManagementSystem.Application.School.Institutes.Models;
using SchoolManagementSystem.Application.School.Institutes.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class InstituteController : ProtectedBaseController
{
    private readonly IWebHostEnvironment _env;
    private static readonly string[] AllowedLogoExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

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
        var logoPath = await SaveLogoAsync(request.Logo);
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
        var logoPath = await SaveLogoAsync(request.Logo);
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

    private async Task<string?> SaveLogoAsync(IFormFile? logo)
    {
        if (logo is null || logo.Length == 0)
        {
            return null;
        }

        var extension = Path.GetExtension(logo.FileName).ToLowerInvariant();
        if (!AllowedLogoExtensions.Contains(extension))
        {
            return null;
        }

        var webRootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
        var uploadFolder = Path.Combine(webRootPath, "uploads", "institutes");
        Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await logo.CopyToAsync(stream);

        return $"/uploads/institutes/{fileName}";
    }
}
