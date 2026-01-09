using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Users.Models;
using SchoolManagementSystem.Application.GS.Users.Queries;

namespace SchoolManagementSystem.Application.GS.Users.Handlers.QueryHandlers;

public class GetUserByIdQueryHandler : IHttpRequestHandler<GetUserByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id != null)
            {
                var result = await _unitOfWork.UserRepository
               .GetAllNoneDeleted(false, true)
               .Where(x => x.Id == request.Id)
               .Include(x => x.UserRoleList.Where(x => x.IsDeleted == false)) // Include the navigation
                   .ThenInclude(x => x.Role) // Then include the Role for RoleName
               .FirstOrDefaultAsync();

                if(result != null)
                {
                    var response = result.Adapt<UserResponse>();

                    // If needed, manually adapt the role list
                    response.UserRoleList = result.UserRoleList?
                        .Select(x => new UserRoleRequest
                        {
                            Id = x.Id,
                            RoleId = x.RoleId,
                            UserId = x.UserId,
                            TenantId = x.TenantId,
                            RoleName = x.Role?.RoleName
                        }).ToList();

                    return Result.Success(response);
                }
            }
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail<UserResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
