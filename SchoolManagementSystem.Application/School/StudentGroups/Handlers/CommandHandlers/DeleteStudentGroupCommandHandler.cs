using SchoolManagementSystem.Application.School.StudentGroups.Commands;

namespace SchoolManagementSystem.Application.School.StudentGroups.Handlers.CommandHandlers;

public class DeleteStudentGroupCommandHandler : IHttpRequestHandler<DeleteStudentGroupCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteStudentGroupCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteStudentGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.StudentGroupRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.StudentGroupRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
