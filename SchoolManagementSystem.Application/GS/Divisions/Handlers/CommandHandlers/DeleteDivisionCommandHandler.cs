using SchoolManagementSystem.Application.GS.Divisions.Commands;

namespace SchoolManagementSystem.Application.GS.Divisions.Handlers.CommandHandlers;
public class DeleteDivisionCommandHandler : IHttpRequestHandler<DeleteDivisionCommand>
{
    private IUnitOfWork _unitOfWork;
    //private ICurrentUserService _currentUserService;
    //private readonly IRolePermissionService _rolePermissionService;
    //private readonly StaticFieldValue _fieldValue;

    public DeleteDivisionCommandHandler(IUnitOfWork unitOfWork) 
    { 
        _unitOfWork = unitOfWork;
        //_currentUserService = currentUserService;
        //_rolePermissionService = rolePermissionService;
        //_fieldValue = fieldValue.Value;
    }

    public async Task<IResult> Handle(DeleteDivisionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            

            var division = await _unitOfWork.DivisionRepository.GetByIdAsync(request.id);
            if (division is null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound, "Division with the given ID does not exist.");
            }

            var result = 
                await _unitOfWork.DivisionRepository.DeleteAsync(division);
            if (result)
            {
                await _unitOfWork.CommitAsync();

                return Result.Success<string>("Successfully deleted", "Division has been successfully deleted.");
            }
            else
            {
                return Result.Fail<string>(StatusCodes.Status500InternalServerError, "An unexpected issue occurred while attempting to delete the division.");
            }
        }
        catch (Exception ex)
        {
           // LogHelpers.Error(ex);
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, "An unexpected error occurred while deleting the division.");
        }
    }
}
