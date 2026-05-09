using SchoolManagementSystem.Application.School.Admissions.Commands;

namespace SchoolManagementSystem.Application.School.Admissions.Handlers.CommandHandlers;

public class DeleteAdmissionCommandHandler : IHttpRequestHandler<DeleteAdmissionCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteAdmissionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteAdmissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.AdmissionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.AdmissionRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
