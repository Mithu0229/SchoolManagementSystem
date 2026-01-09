using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Sitemaps.Models;
using System.Data;
using System.Text.Json;

namespace SchoolManagementSystem.Infrastructure.Common;
public class PagedService : IPagedService
{
    private readonly IConfiguration _config;
    public PagedService(IConfiguration config)
    {
        _config = config;
    }

    #region new service

    public Task<PagedResult<T>> GetPagedAsync<T>(string sp, string countSp, PagedRequest request, bool includeTenant, bool includeFiltersText)
    {
        var (parameters, countparameters) = BuildDynamicParameters(request, includeTenant, includeFiltersText);


        return ExecutePagedStoredProcAsync<T>(sp, countSp, parameters, countparameters, request.Page, request.PageSize);
    }

    public Task<PagedResult<T>> GetPagedAsync<T>(string functionName, string countFunction, DynamicParameters parameters, DynamicParameters countParameters, int page, int pageSize)
    {
        return ExecutePagedStoredProcAsync<T>(functionName, countFunction, parameters, countParameters, page, pageSize);
    }


    private async Task<PagedResult<T>> ExecutePagedStoredProcAsync<T>(
        string dataProcName,
        string countProcName,
        DynamicParameters parameters,
        DynamicParameters countParameters,
        int page,
        int pageSize)
    {
        try
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            var mergedParams = new DynamicParameters();
            foreach (var name in parameters.ParameterNames)
                mergedParams.Add(name, parameters.Get<object>(name));

            foreach (var name in countParameters.ParameterNames)
                if (!mergedParams.ParameterNames.Contains(name))
                    mergedParams.Add(name, countParameters.Get<object>(name));
            
            // Count query – must use only countParameters
            var totalCount = await conn.ExecuteScalarAsync<int>(
                countProcName,
                countParameters,
                commandType: CommandType.StoredProcedure);

            // Data query
            var items = (await conn.QueryAsync<T>(
                dataProcName,
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();


            return new PagedResult<T>
            {
                Items = items,
                TotalRecord = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion



    #region Helpers

    private (DynamicParameters parameters, DynamicParameters countParameters) BuildDynamicParameters(PagedRequest request, bool includeTenant, bool includeFiltersText)
    {
        var parameters = new DynamicParameters();
        var countParameters = new DynamicParameters();
        parameters.Add("Page", request.Page);
        parameters.Add("Length", request.PageSize);
        parameters.Add("Sort", request.SortColumn ?? "");
        parameters.Add("Direction", request.SortDirection ?? "");
        parameters.Add("Search", request.Search!.Trim() ?? "");
        countParameters.Add("Search", request.Search!.Trim() ?? "");
        if (includeFiltersText)
        {
            NormalizeFilterValues(request.Filters);
            var filtersJson = JsonConvert.SerializeObject(request.Filters);
            parameters.Add("Filters_text", filtersJson);
            countParameters.Add("Filters_text", filtersJson);
        }
        if (includeTenant)
        {
            parameters.Add("TenantId", null, DbType.Guid);
            countParameters.Add("TenantId", null, DbType.Guid);
        }
        return (parameters, countParameters);
    }

    private void NormalizeFilterValues(List<Filter>? filters)
    {
        if (filters == null) return;
        foreach (var filter in filters)
        {
            if (filter.Value is JsonElement element)
            {
                filter.Value = element.ValueKind switch
                {
                    JsonValueKind.String => element.GetString(),
                    JsonValueKind.Number => element.TryGetInt32(out var intVal) ? intVal : element.GetDouble(),
                    JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
                    JsonValueKind.Null => null,
                    _ => element.ToString()
                };
            }
        }
    }


    #endregion

}
