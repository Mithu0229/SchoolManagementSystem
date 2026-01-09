using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Tenants.Queries;

namespace SchoolManagementSystem.Application.GS.Tenants.Handlers.QueryHandlers;

public class GetTenantDropdownQueryHandler : IHttpRequestHandler<GetTenantDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetTenantDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> Handle(GetTenantDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _unitOfWork.TenantRepository.GetAllNoneDeleted().Where(item => item.IsActive).Select(s => new
            {
                s.Id,
                s.TenantName,
            }).OrderBy(o => o.TenantName).ToListAsync(cancellationToken);
            if (result == null || !result.Any())
                return Result.Fail(StatusCodes.Status404NotFound, "No tentant found.");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<string>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
