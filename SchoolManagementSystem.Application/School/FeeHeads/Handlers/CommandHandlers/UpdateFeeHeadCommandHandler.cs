using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeHeads.Commands;
using SchoolManagementSystem.Application.School.FeeHeads.Models;

namespace SchoolManagementSystem.Application.School.FeeHeads.Handlers.CommandHandlers;

public class UpdateFeeHeadCommandHandler : IHttpRequestHandler<UpdateFeeHeadCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateFeeHeadCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateFeeHeadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.FeeHead.Id == Guid.Empty) return Result.Fail<FeeHeadResponse>(StatusCodes.Status406NotAcceptable);
            request.FeeHead.FeeHeadName = request.FeeHead.FeeHeadName.Trim();
            var entity = await _unitOfWork.FeeHeadRepository.GetSingleNoneDeletedAsync(x => x.Id == request.FeeHead.Id);
            if (entity is null) return Result.Fail<FeeHeadResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.FeeHeadRepository.GetAllNoneDeleted().Where(x => x.Id != request.FeeHead.Id && x.FeeHeadName.ToLower() == request.FeeHead.FeeHeadName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Fee head already exists!");
            entity.FeeHeadName = request.FeeHead.FeeHeadName;
            entity.IsMonthly = request.FeeHead.IsMonthly;
            entity.IsActive = request.FeeHead.IsActive;
            await _unitOfWork.FeeHeadRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<FeeHeadResponse>(), "FeeHead " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<FeeHeadResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
