using SchoolManagementSystem.Application.School.BillMasters.Commands;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.CommandHandlers;

public class DeleteBillMasterCommandHandler : IHttpRequestHandler<DeleteBillMasterCommand>
{
    private IUnitOfWork _unitOfWork;
    public DeleteBillMasterCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(DeleteBillMasterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty) return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.BillMasterRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);
            if (entity is null) return Result.Fail<string>(StatusCodes.Status404NotFound);
            await _unitOfWork.BillMasterRepository.InstantDeleteWithDeactivate(entity);
            await _unitOfWork.BillMasterRepository.ReplaceManyAsync<BillDetail>(x => x.BillMasterId == request.id, new List<BillDetail>());
            return Result.Success("Succefully deleted");
        }
        catch (Exception ex) { return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
