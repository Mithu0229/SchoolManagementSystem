using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FinancialYears.Queries;

namespace SchoolManagementSystem.Application.School.FinancialYears.Handlers.QueryHandlers;

public class GetFinancialYearDropdownQueryHandler : IHttpRequestHandler<GetFinancialYearDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFinancialYearDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetFinancialYearDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.FinancialYearRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.FinYearName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
