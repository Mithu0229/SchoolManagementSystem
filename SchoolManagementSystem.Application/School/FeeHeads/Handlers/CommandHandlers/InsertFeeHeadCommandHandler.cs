using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeHeads.Commands;
using SchoolManagementSystem.Application.School.FeeHeads.Models;

namespace SchoolManagementSystem.Application.School.FeeHeads.Handlers.CommandHandlers;

public class InsertFeeHeadCommandHandler : IHttpRequestHandler<InsertFeeHeadCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertFeeHeadCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertFeeHeadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<FeeHeadResponse>(StatusCodes.Status406NotAcceptable);
            request.FeeHead.FeeHeadName = request.FeeHead.FeeHeadName.Trim();
            var duplicate = await _unitOfWork.FeeHeadRepository.GetAllNoneDeleted().Where(x => x.FeeHeadName.ToLower() == request.FeeHead.FeeHeadName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Fee head already exists!");
            var entity = request.FeeHead.Adapt<FeeHead>();
            await _unitOfWork.FeeHeadRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<FeeHeadResponse>(), "FeeHead " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<FeeHeadResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
