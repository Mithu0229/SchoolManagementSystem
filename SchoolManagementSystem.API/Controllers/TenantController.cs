using SchoolManagementSystem.Application.GS.Tenants.Commands;
using SchoolManagementSystem.Application.GS.Tenants.Models;
using SchoolManagementSystem.Application.GS.Tenants.Queries;

namespace SchoolManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ProtectedBaseController
    {
        [HttpPost("save-tenant")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TenantResponse))]
        public Task<IResult> Post([FromBody] TenantRequest request)
        {
            InsertTenantCommand cmd = new InsertTenantCommand() { Tenant = request };
            return Mediator.Send(cmd);
        }

        [HttpPost("get-tenant-list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TenantResponse))]
        public async Task<IResult> GetTenantList([FromBody] PagedRequest request)
        {
            return await Mediator.Send(new GetTenantListQuery() { PagedRequest = request });

        }
    }

}
