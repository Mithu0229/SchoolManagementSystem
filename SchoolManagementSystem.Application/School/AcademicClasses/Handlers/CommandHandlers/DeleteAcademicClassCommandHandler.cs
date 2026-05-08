using SchoolManagementSystem.Application.School.AcademicClasses.Commands;

namespace SchoolManagementSystem.Application.School.AcademicClasses.Handlers.CommandHandlers;

public class DeleteAcademicClassCommandHandler : IHttpRequestHandler<DeleteAcademicClassCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteAcademicClassCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteAcademicClassCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }

            var entity = await _unitOfWork.AcademicClassRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound);
            }

            await _unitOfWork.AcademicClassRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
