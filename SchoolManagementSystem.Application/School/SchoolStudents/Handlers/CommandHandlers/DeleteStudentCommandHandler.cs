using SchoolManagementSystem.Application.School.SchoolStudents.Commands;

namespace SchoolManagementSystem.Application.School.SchoolStudents.Handlers.CommandHandlers;

public class DeleteStudentCommandHandler : IHttpRequestHandler<DeleteStudentCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteStudentCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.StudentRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.StudentRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
