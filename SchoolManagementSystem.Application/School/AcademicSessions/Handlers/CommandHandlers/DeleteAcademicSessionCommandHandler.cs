using SchoolManagementSystem.Application.School.AcademicSessions.Commands;

namespace SchoolManagementSystem.Application.School.AcademicSessions.Handlers.CommandHandlers;

public class DeleteAcademicSessionCommandHandler : IHttpRequestHandler<DeleteAcademicSessionCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteAcademicSessionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteAcademicSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }

            var entity = await _unitOfWork.AcademicSessionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound);
            }

            await _unitOfWork.AcademicSessionRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
