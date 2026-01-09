using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Common;
public interface IPagedService
{
    Task<PagedResult<T>> GetPagedAsync<T>(string functionName, string countFunction, PagedRequest request, bool includeTenant, bool includeFiltersText);
    Task<PagedResult<T>> GetPagedAsync<T>(string functionName, string countFunction, DynamicParameters parameters, DynamicParameters countParameters, int page, int pageSize);
}