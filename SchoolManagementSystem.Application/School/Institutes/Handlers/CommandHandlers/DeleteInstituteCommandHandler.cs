using SchoolManagementSystem.Application.School.Institutes.Commands;

namespace SchoolManagementSystem.Application.School.Institutes.Handlers.CommandHandlers;

public class DeleteInstituteCommandHandler : IHttpRequestHandler<DeleteInstituteCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteInstituteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteInstituteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }

            var entity = await _unitOfWork.InstituteRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound);
            }

            await _unitOfWork.InstituteRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
