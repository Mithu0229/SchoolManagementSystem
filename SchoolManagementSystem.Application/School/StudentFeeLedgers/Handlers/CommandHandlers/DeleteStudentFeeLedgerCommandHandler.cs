using SchoolManagementSystem.Application.School.StudentFeeLedgers.Commands;

namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Handlers.CommandHandlers;

public class DeleteStudentFeeLedgerCommandHandler : IHttpRequestHandler<DeleteStudentFeeLedgerCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteStudentFeeLedgerCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteStudentFeeLedgerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.StudentFeeLedgerRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.StudentFeeLedgerRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
