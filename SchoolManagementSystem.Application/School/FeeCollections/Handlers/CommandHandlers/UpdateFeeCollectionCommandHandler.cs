using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeCollections.Commands;
using SchoolManagementSystem.Application.School.FeeCollections.Models;

namespace SchoolManagementSystem.Application.School.FeeCollections.Handlers.CommandHandlers;

public class UpdateFeeCollectionCommandHandler : IHttpRequestHandler<UpdateFeeCollectionCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateFeeCollectionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateFeeCollectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.FeeCollection.Id == Guid.Empty) return Result.Fail<FeeCollectionResponse>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.FeeCollectionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.FeeCollection.Id);
            if (entity is null) return Result.Fail<FeeCollectionResponse>(StatusCodes.Status404NotFound);
            if (!string.IsNullOrWhiteSpace(request.FeeCollection.MemoNo))
            {
                request.FeeCollection.MemoNo = request.FeeCollection.MemoNo.Trim();
                var duplicate = await _unitOfWork.FeeCollectionRepository.GetAllNoneDeleted().Where(x => x.Id != request.FeeCollection.Id && x.MemoNo != null && x.MemoNo.ToLower() == request.FeeCollection.MemoNo.ToLower()).FirstOrDefaultAsync(cancellationToken);
                if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Memo no already exists!");
            }
            entity.CollectionDate = request.FeeCollection.CollectionDate;
            entity.MemoNo = request.FeeCollection.MemoNo;
            entity.StudentId = request.FeeCollection.StudentId;
            entity.AdmissionId = request.FeeCollection.AdmissionId;
            entity.BranchId = request.FeeCollection.BranchId;
            entity.FinancialYearId = request.FeeCollection.FinancialYearId;
            entity.TotalAmount = request.FeeCollection.TotalAmount;
            entity.DiscountAmount = request.FeeCollection.DiscountAmount;
            entity.PaidAmount = request.FeeCollection.PaidAmount;
            entity.DueAmount = request.FeeCollection.DueAmount;
            entity.PaymentMode = request.FeeCollection.PaymentMode;
            entity.Remarks = request.FeeCollection.Remarks;
            entity.IsCancelled = request.FeeCollection.IsCancelled;
            entity.IsActive = request.FeeCollection.IsActive;
            await _unitOfWork.FeeCollectionRepository.UpdateAsync(entity);
            var details = request.FeeCollection.Details.Select(x => new FeeCollectionDetail
            {
                FeeCollectionId = entity.Id,
                FeeHeadId = x.FeeHeadId,
                MonthNo = x.MonthNo,
                YearNo = x.YearNo,
                FeeAmount = x.FeeAmount,
                DiscountAmount = x.DiscountAmount,
                PaidAmount = x.PaidAmount,
                DueAmount = x.DueAmount
            }).ToList();
            await _unitOfWork.FeeCollectionRepository.ReplaceManyAsync<FeeCollectionDetail>(x => x.FeeCollectionId == entity.Id, details);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<FeeCollectionResponse>(), "FeeCollection " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<FeeCollectionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
