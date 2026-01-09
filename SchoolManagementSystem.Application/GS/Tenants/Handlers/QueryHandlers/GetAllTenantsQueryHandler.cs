using SchoolManagementSystem.Application.GS.Tenants.Models;
using SchoolManagementSystem.Application.GS.Tenants.Queries;
using System.Text.Json;

namespace SchoolManagementSystem.Application.GS.Tenants.Handlers.QueryHandlers;

public class GetAllTenantsQueryHandler : IHttpRequestHandler<GetAllTenantsQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTenantsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<TenantResponse> tenants = new List<TenantResponse>();
            int start = (request.pg - 1) * request.sl;
            int length = request.sl;
            string? searchValue = request.sv != null ? request.sv.ToLower() : request.sv;
            var parameters = new Dictionary<string, object>
            {
                { "p_pg", request.pg! },
                { "p_len", length! },
                { "p_sv", searchValue! }
            };
            tenants = (await _unitOfWork.DapperCommandQuery.GetDataListAsync<TenantResponse>
                ("Select * From fngetettenantlist(@p_pg, @p_len, @p_sv);", parameters, System.Data.CommandType.Text)).ToList();
            var Data = JsonSerializer.Serialize(new
            {
                items = tenants
                //recordsTotal = tenants.Select(x => x.TotalRecord).FirstOrDefault(),
                //recordsFiltered = tenants.Select(x => x.FilteredRecord).FirstOrDefault()
            });
            return Result.Success(tenants);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex.InnerException.Message);
            return Result.Fail<IList<TenantResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
