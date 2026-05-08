using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FinancialYears.Commands;
using SchoolManagementSystem.Application.School.FinancialYears.Models;

namespace SchoolManagementSystem.Application.School.FinancialYears.Handlers.CommandHandlers;

public class InsertFinancialYearCommandHandler : IHttpRequestHandler<InsertFinancialYearCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertFinancialYearCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(InsertFinancialYearCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<FinancialYearResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.FinancialYear.FinYearName = request.FinancialYear.FinYearName.Trim();
            var duplicate = await _unitOfWork.FinancialYearRepository.GetAllNoneDeleted()
                .Where(x => x.FinYearName.ToLower() == request.FinancialYear.FinYearName.ToLower() || x.FinCode == request.FinancialYear.FinCode)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Financial year already exists!");
            }

            var entity = request.FinancialYear.Adapt<FinancialYear>();
            await _unitOfWork.FinancialYearRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<FinancialYearResponse>();
            return Result.Success(response, "FinancialYear " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<FinancialYearResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
