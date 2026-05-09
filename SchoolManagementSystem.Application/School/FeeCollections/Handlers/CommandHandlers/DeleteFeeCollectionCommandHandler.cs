using SchoolManagementSystem.Application.School.FeeCollections.Commands;

namespace SchoolManagementSystem.Application.School.FeeCollections.Handlers.CommandHandlers;

public class DeleteFeeCollectionCommandHandler : IHttpRequestHandler<DeleteFeeCollectionCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteFeeCollectionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteFeeCollectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.FeeCollectionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.FeeCollectionRepository.InstantDeleteWithDeactivate(entity);
            await _unitOfWork.FeeCollectionRepository.ReplaceManyAsync<FeeCollectionDetail>(x => x.FeeCollectionId == request.id, new List<FeeCollectionDetail>());
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
