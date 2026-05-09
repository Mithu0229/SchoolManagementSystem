using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeCollections.Models;
using SchoolManagementSystem.Application.School.FeeCollections.Queries;

namespace SchoolManagementSystem.Application.School.FeeCollections.Handlers.QueryHandlers;

public class GetFeeCollectionByIdQueryHandler : IHttpRequestHandler<GetFeeCollectionByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFeeCollectionByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetFeeCollectionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<FeeCollectionResponse>(StatusCodes.Status406NotAcceptable);
            var response = await _unitOfWork.FeeCollectionRepository.GetAllNoneDeleted(true).Where(x => x.Id == request.Id).Select(x => new FeeCollectionResponse
            {
                Id = x.Id,
                CollectionDate = x.CollectionDate,
                MemoNo = x.MemoNo,
                StudentId = x.StudentId,
                AdmissionId = x.AdmissionId,
                BranchId = x.BranchId,
                FinancialYearId = x.FinancialYearId,
                TotalAmount = x.TotalAmount,
                DiscountAmount = x.DiscountAmount,
                PaidAmount = x.PaidAmount,
                DueAmount = x.DueAmount,
                PaymentMode = x.PaymentMode,
                Remarks = x.Remarks,
                IsCancelled = x.IsCancelled,
                IsActive = x.IsActive,
                Details = x.Details.Where(d => !d.IsDeleted).Select(d => new FeeCollectionDetailResponse
                {
                    Id = d.Id,
                    FeeCollectionId = d.FeeCollectionId,
                    FeeHeadId = d.FeeHeadId,
                    FeeHeadName = d.FeeHead.FeeHeadName,
                    MonthNo = d.MonthNo,
                    YearNo = d.YearNo,
                    FeeAmount = d.FeeAmount,
                    DiscountAmount = d.DiscountAmount,
                    PaidAmount = d.PaidAmount,
                    DueAmount = d.DueAmount
                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);
            if (response is null) return Result.Fail<FeeCollectionResponse>(StatusCodes.Status404NotFound);
            return Result.Success(response);
        }
        catch (Exception ex) { return Result.Fail<FeeCollectionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
