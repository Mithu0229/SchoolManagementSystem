using SchoolManagementSystem.Application.School.Branches.Commands;

namespace SchoolManagementSystem.Application.School.Branches.Handlers.CommandHandlers;

public class DeleteBranchCommandHandler : IHttpRequestHandler<DeleteBranchCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteBranchCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }

            var entity = await _unitOfWork.BranchRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound);
            }

            await _unitOfWork.BranchRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
