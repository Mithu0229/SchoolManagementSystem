using SchoolManagementSystem.Application.School.FeeHeads.Commands;

namespace SchoolManagementSystem.Application.School.FeeHeads.Handlers.CommandHandlers;

public class DeleteFeeHeadCommandHandler : IHttpRequestHandler<DeleteFeeHeadCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteFeeHeadCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteFeeHeadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.FeeHeadRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.FeeHeadRepository.InstantDeleteWithDeactivate(entity);
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
