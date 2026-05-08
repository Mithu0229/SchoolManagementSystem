using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FinancialYears.Commands;
using SchoolManagementSystem.Application.School.FinancialYears.Models;

namespace SchoolManagementSystem.Application.School.FinancialYears.Handlers.CommandHandlers;

public class UpdateFinancialYearCommandHandler : IHttpRequestHandler<UpdateFinancialYearCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateFinancialYearCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateFinancialYearCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.FinancialYear.Id == Guid.Empty)
            {
                return Result.Fail<FinancialYearResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.FinancialYear.FinYearName = request.FinancialYear.FinYearName.Trim();
            var entity = await _unitOfWork.FinancialYearRepository.GetSingleNoneDeletedAsync(x => x.Id == request.FinancialYear.Id);
            if (entity is null)
            {
                return Result.Fail<FinancialYearResponse>(StatusCodes.Status404NotFound);
            }

            var duplicate = await _unitOfWork.FinancialYearRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.FinancialYear.Id && (x.FinYearName.ToLower() == request.FinancialYear.FinYearName.ToLower() || x.FinCode == request.FinancialYear.FinCode))
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Financial year already exists!");
            }

            entity.FinYearName = request.FinancialYear.FinYearName;
            entity.FromDate = request.FinancialYear.FromDate;
            entity.ToDate = request.FinancialYear.ToDate;
            entity.FinCode = request.FinancialYear.FinCode;
            entity.Remarks = request.FinancialYear.Remarks;
            entity.IsCurrent = request.FinancialYear.IsCurrent;
            entity.IsActive = request.FinancialYear.IsActive;

            await _unitOfWork.FinancialYearRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<FinancialYearResponse>();
            return Result.Success(response, "FinancialYear " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<FinancialYearResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
