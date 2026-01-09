using MediatR;

namespace SchoolManagementSystem.API.Controllers;

[Route("api/[controller]"), ApiController]
public class PublicBaseController : ControllerBase
{
    private ISender? _sender;
    internal ISender Mediator => (_sender ??= HttpContext.RequestServices.GetService<ISender>())!;
}
