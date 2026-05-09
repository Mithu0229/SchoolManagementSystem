using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeCollections.Commands;
using SchoolManagementSystem.Application.School.FeeCollections.Models;

namespace SchoolManagementSystem.Application.School.FeeCollections.Handlers.CommandHandlers;

public class InsertFeeCollectionCommandHandler : IHttpRequestHandler<InsertFeeCollectionCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertFeeCollectionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertFeeCollectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<FeeCollectionResponse>(StatusCodes.Status406NotAcceptable);
            if (!string.IsNullOrWhiteSpace(request.FeeCollection.MemoNo))
            {
                request.FeeCollection.MemoNo = request.FeeCollection.MemoNo.Trim();
                var duplicate = await _unitOfWork.FeeCollectionRepository.GetAllNoneDeleted().Where(x => x.MemoNo != null && x.MemoNo.ToLower() == request.FeeCollection.MemoNo.ToLower()).FirstOrDefaultAsync(cancellationToken);
                if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Memo no already exists!");
            }
            var entity = request.FeeCollection.Adapt<FeeCollection>();
            entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
            entity.Details = new List<FeeCollectionDetail>();
            await _unitOfWork.FeeCollectionRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            if (request.FeeCollection.Details.Count > 0)
            {
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
            }
            return Result.Success(entity.Adapt<FeeCollectionResponse>(), "FeeCollection " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<FeeCollectionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
