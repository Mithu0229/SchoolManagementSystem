using SchoolManagementSystem.Application.School.Sections.Commands;

namespace SchoolManagementSystem.Application.School.Sections.Handlers.CommandHandlers;

public class DeleteSectionCommandHandler : IHttpRequestHandler<DeleteSectionCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteSectionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.SectionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.SectionRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
