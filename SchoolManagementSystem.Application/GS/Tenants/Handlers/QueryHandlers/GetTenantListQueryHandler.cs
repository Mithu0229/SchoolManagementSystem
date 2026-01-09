using SchoolManagementSystem.Application.GS.Tenants.Models;
using SchoolManagementSystem.Application.GS.Tenants.Queries;

namespace SchoolManagementSystem.Application.GS.Handlers.QueryHandlers;

public class GetTenantListQueryHandler : IHttpRequestHandler<GetTenantListQuery>
{
    private IPagedService _pagedService;
    public GetTenantListQueryHandler(IPagedService pagedService)
    {
        _pagedService = pagedService;
    }
    public async Task<IResult> Handle(GetTenantListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _pagedService.GetPagedAsync<TenantResponse>("dbo.sp_GetTenants","dbo.sp_GetTenantsCount", request.PagedRequest!, false, true);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<TenantResponse>>(StatusCodes.Status500InternalServerError, ex.InnerException!.Message);
        }
    }

}

