using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Common;
public interface IDapperCommandQuery
{
    Task<CustomReponse> RunSpAsync(string spName, bool isParamExists, string param);
    Task<CustomReponse> RunSpAsync(string spName, int pg, int length, string searchVal, object value);
    Task<CustomReponse> RunSpAsync(string spName, int flag, string pageUrl, Guid targetId);
    Task<IEnumerable<T>> GetDataListAsync<T>(string Query, IDictionary<string, object> parameters, CommandType commandType, string? SQLConnectionstr = null);
    Task<int> ExecuteAsync(string Query, IDictionary<string, object> parameters, CommandType commandType, string? SQLConnectionstr = null);
    Task<string> ExecuteScalarAsync(string Query, IDictionary<string, object> parameters, CommandType commandType, string? SQLConnectionstr = null);
    Task<DataSet> GetDataSetAsync(string Query, IDictionary<string, object> parameters, CommandType commandType, string? SQLConnectionstr = null);
    Task<List<T>> GetDataListAsync<T>(string query, object value, CommandType text);
    Task<PagedResult<T>> GetPagedAsync<T>(string storedProcedure, PagedRequest request);
}

