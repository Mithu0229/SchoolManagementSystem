using SchoolManagementSystem.Application.GS.Users.Commands;

namespace SchoolManagementSystem.Application.GS.Users.Handlers.CommandHandlers;
public class DeleteUserCommandHandler : IHttpRequestHandler<DeleteUserCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteUserCommandHandler(IUnitOfWork unitOfWork) 
    { 
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {

            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }

            var user = await _unitOfWork.UserRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (user == null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound);
            }
            var result = await _unitOfWork.UserRepository.InstantDelete(user);

            if (result)
            {
                return Result.Success<string>("Successfully Deleted");
            }
            else
            {
                return Result.Fail<string>("Failed to Delete");
            }
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, "An unexpected error occurred while deleting the User.",ex.Message);
        }
    }
}
