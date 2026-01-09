using SchoolManagementSystem.Application.GS.Roles.Commands;

namespace SchoolManagementSystem.Application.GS.Roles.Handlers.CommandHandlers;
public class DeleteRoleCommandHandler : IHttpRequestHandler<DeleteRoleCommand>
{
    private IUnitOfWork _unitOfWork;
    
    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork) 
    { 
        _unitOfWork = unitOfWork;
        
    }
    public async Task<IResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var role = await _unitOfWork.RoleRepository.GetSingleNoneDeletedAsync(x=>x.Id==request.id);
            if (role is null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound, "Role with the given ID does not exist.");
            }

            var result = role.Adapt<Role>();//await _unitOfWork.RoleRepository.InstantDeleteAsync(role,false);
            if (result !=null)
            {
                return Result.Success<string>("Successfully deleted", "Role has been successfully deleted.");
            }
            else
            {
                return Result.Fail<string>(StatusCodes.Status500InternalServerError, "An unexpected issue occurred while attempting to delete the role.");
            }
        }
        catch (Exception ex)
        {
           // LogHelpers.Error(ex);
            return Result.Fail<string>(StatusCodes.Status500InternalServerError,ex.Message+ "An unexpected error occurred while deleting the division.");
        }
    }
}
