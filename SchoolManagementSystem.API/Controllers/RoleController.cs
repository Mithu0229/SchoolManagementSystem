using MediatR;
using SchoolManagementSystem.Application.GS.Roles.Commands;
using SchoolManagementSystem.Application.GS.Roles.Models;
using SchoolManagementSystem.Application.GS.Roles.Queries;

namespace SchoolManagementSystem.API.Controllers
{
    public class RoleController : PublicBaseController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleWithMenuPermissionResponse))]
        public Task<IResult> Get(Guid id)
        {
            return Mediator.Send(new GetRoleByIdQuery(id));//
        }

        [HttpPost("save-role")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponse))]
        public async Task<IResult> Post([FromBody] RoleWithMenuPersmissionRequest request)
        {
            InsertRoleCommand cmd = new InsertRoleCommand() { RolePermission = request };
            return await Mediator.Send(cmd);//ok
        }

        [HttpPut("update-role")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponse))]
        public async Task<IResult> Put([FromBody] RoleWithMenuPersmissionRequest request)
        {
            var Id = Guid.Parse("9b4d7618-3f78-40ce-ab1a-487edfc9d2bd"); //Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UpdateRoleCommand cmd = new UpdateRoleCommand() { RolePermission = request };
            return await Mediator.Send(cmd);
        }

        //[HttpDelete("delete-role")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponse))]
        //public async Task<IResult> DeleteRoleRequest(DeleteRoleRequest request)
        //{
        //    DeleteRoleRequestCommand cmd = new DeleteRoleRequestCommand() { DeleteRole = request };
        //    return await Mediator.Send(cmd);
        //}

        //[HttpPost("cancel-delete-request")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponse))]
        //public async Task<IResult> CancelRoleDeletion(DeleteRoleRequest request)
        //{
        //    CancelDeleteRoleRequestCommand cmd = new CancelDeleteRoleRequestCommand() { DeleteRole = request };
        //    return await Mediator.Send(cmd);
        //}


        [HttpPost(("get-role-list"))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponse))]
        public async Task<IResult> GetRolesPaged([FromBody] PagedRequest request)
        {
            return await Mediator.Send(new GetRolesPagesQuery() { RolesPaged = request });//
        }

        [HttpGet("get-role-by-teanantId/{id}")]
        public Task<IResult> GetRoleByTenant(Guid? id)
        {
            GetRoleByTenantQuery cmd = new GetRoleByTenantQuery(id);
            return Mediator.Send(cmd);
        }

        //[HttpGet("get-user-count-by-roleId")]
        //public Task<IResult> GetUserByRole([FromQuery] Guid id)
        //{
        //    GetUserByRoleIdQuery cmd = new GetUserByRoleIdQuery(id);
        //    return Mediator.Send(cmd);
        //}

        //[HttpPost("transfer-role")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponse))]
        //public async Task<IResult> TransferRole([FromBody] RoleTransferRequest request)
        //{
        //    TransferRoleCommand cmd = new TransferRoleCommand() { RoleTransfer = request };
        //    return await Mediator.Send(cmd);
        //}

        //[HttpGet("verify-role-transfer/{userId}")]
        //public Task<IResult> VerifyRoleTransfer(Guid userId)
        //{
        //    VerifyRoleTransferQuery cmd = new VerifyRoleTransferQuery(userId);
        //    return Mediator.Send(cmd);
        //}

        //[HttpPost("cancel-role-transfer")]
        //public Task<IResult> CancelRoleTransfer([FromBody] CancelRoleTransferRequest request)
        //{
        //    CancelRoleTransferCommand cmd = new CancelRoleTransferCommand() { CancelRoleTransfer = request };
        //    return Mediator.Send(cmd);
        //}

        //[HttpGet("role-transfer-activation")]
        //public Task<IResult> ActivateRoleTransfer([FromQuery] string key)
        //{
        //   // ActivateRoleTransferCommand cmd = new ActivateRoleTransferCommand(key);
        //    return Mediator.Send(cmd);
        //}

    }

    }

