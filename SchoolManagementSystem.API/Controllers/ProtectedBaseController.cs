using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.API.Controllers;


[Route("api/[controller]"), ApiController, Authorize]
public class ProtectedBaseController : ControllerBase
{
    private ISender? _sender;
    internal ISender Mediator => (_sender ??= HttpContext.RequestServices.GetService<ISender>())!;
}
