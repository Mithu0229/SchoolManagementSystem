using SchoolManagementSystem.Application.School.FinancialYears.Models;
using SchoolManagementSystem.Application.School.FinancialYears.Queries;

namespace SchoolManagementSystem.Application.School.FinancialYears.Handlers.QueryHandlers;

public class GetFinancialYearByIdQueryHandler : IHttpRequestHandler<GetFinancialYearByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFinancialYearByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetFinancialYearByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                return Result.Fail<FinancialYearResponse>(StatusCodes.Status406NotAcceptable);
            }

            var result = await _unitOfWork.FinancialYearRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null)
            {
                return Result.Fail<FinancialYearResponse>(StatusCodes.Status404NotFound);
            }

            var response = result.Adapt<FinancialYearResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<FinancialYearResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
