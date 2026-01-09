using SchoolManagementSystem.Application.GS.Users.Commands;
using SchoolManagementSystem.Application.GS.Users.Models;
using SchoolManagementSystem.Application.GS.Users.Queries;

namespace SchoolManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : PublicBaseController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        public Task<IResult> Get(Guid id)
        {
            return Mediator.Send(new GetUserByIdQuery(id));
        }
        [HttpPost("get-user-list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        public async Task<IResult> GetUserList([FromBody] PagedRequest request)
        {
            return await Mediator.Send(new GetUserListQuery() { PagedRequest = request });

        }

        [HttpPost("save-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        public Task<IResult> Post([FromBody] UserRequest request)
        {
            InsertUserCommand cmd = new InsertUserCommand() { User = request };
            return Mediator.Send(cmd);
        }

        [HttpPut("update-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        public Task<IResult> Put([FromBody] UserRequest request)
        {
            UpdateUserCommand cmd = new UpdateUserCommand() { User = request };
            return Mediator.Send(cmd);
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IResult> DeleteUser(Guid id)
        {
            DeleteUserCommand cmd = new DeleteUserCommand(id);
            return await Mediator.Send(cmd);
        }

        [HttpPost("user-login")]
        public async Task<IResult> UserLogin(LoginUserRequest request)
        {
            UserLoginCommand cmd = new UserLoginCommand() { LoginUser = request };
            return await Mediator.Send(cmd);
        }

    }
}
