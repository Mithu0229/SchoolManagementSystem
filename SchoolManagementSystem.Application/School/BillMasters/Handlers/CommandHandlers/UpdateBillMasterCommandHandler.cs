using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.BillMasters.Commands;
using SchoolManagementSystem.Application.School.BillMasters.Models;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.CommandHandlers;

public class UpdateBillMasterCommandHandler : IHttpRequestHandler<UpdateBillMasterCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateBillMasterCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateBillMasterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.BillMaster.Id == Guid.Empty) return Result.Fail<BillMasterResponse>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.BillMasterRepository.GetSingleNoneDeletedAsync(x => x.Id == request.BillMaster.Id);
            if (entity is null) return Result.Fail<BillMasterResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.BillMaster.Id && x.AdmissionId == request.BillMaster.AdmissionId && x.BillMonth == request.BillMaster.BillMonth && x.BillYear == request.BillMaster.BillYear)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Bill already exists for this admission, month and year!");
            entity.AdmissionId = request.BillMaster.AdmissionId;
            entity.BillMonth = request.BillMaster.BillMonth;
            entity.BillYear = request.BillMaster.BillYear;
            entity.TotalAmount = request.BillMaster.TotalAmount;
            entity.IsActive = request.BillMaster.IsActive;
            await _unitOfWork.BillMasterRepository.UpdateAsync(entity);
            var details = request.BillMaster.Details.Select(x => new BillDetail
            {
                BillMasterId = entity.Id,
                FeeTemplateDetailId = x.FeeTemplateDetailId,
                FeeHeadId = x.FeeHeadId,
                Amount = x.Amount
            }).ToList();
            await _unitOfWork.BillMasterRepository.ReplaceManyAsync<BillDetail>(x => x.BillMasterId == entity.Id, details);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<BillMasterResponse>(), "BillMaster " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<BillMasterResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
