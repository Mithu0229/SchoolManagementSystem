using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using System.Collections;
using System.Data;
using System.Net.Sockets;
using System.Text.Json;

namespace SchoolManagementSystem.Infrastructure.Common;
public class DapperCommandQuery : IDapperCommandQuery
{
    private readonly ApplicationDbContext _context;
    public DapperCommandQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomReponse> RunSpAsync(string spName, bool IsparametersExists, string param)
    {
        try
        {

            var results = new object();
            using (var con = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await con.OpenAsync();
                if (IsparametersExists)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", param!);
                    results = await con.QueryAsync<object>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    results = await con.QueryAsync<object>(spName, param);

                }
                await con.CloseAsync();
            }
            return new CustomReponse
            {
                Data = JsonSerializer.Serialize(new
                {
                    Data = results,
                })
            };

        }
        catch (Exception ex)
        {
            //_logger.LogError(ex.Message + " ~ " + ex.InnerException);
            throw;
        }
    }

    public async Task<CustomReponse> RunSpAsync(string spName, int pg, int length, string searchVal, object value)
    {
        try
        {

            var results = new object();
            int totalRecords = 0;
            int filteredRecords = 0;
            using (var con = new SqlConnection(_context.Database.GetConnectionString()))
            {
                con.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@pg", pg!);
                parameters.Add("@len", length!);
                parameters.Add("@sv", searchVal!);
                parameters.Add("@totalrows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                results = await con.QueryAsync<object>(spName, parameters);
                totalRecords = parameters.Get<int>("totalrows");
                IEnumerable<object> enumerable = ((IEnumerable)results).Cast<object>();
                filteredRecords = enumerable.Count();
                con.Close();
            }
            return new CustomReponse
            {
                Data = JsonSerializer.Serialize(new
                {
                    Data = results,
                }),
                TotalRecords = totalRecords,
                FilteredRecords = filteredRecords
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<CustomReponse> RunSpAsync(string spName, int flag, string pageUrl, Guid targetId)
    {
        try
        {

            var results = new object();
         
            using (var con = new SqlConnection(_context.Database.GetConnectionString()))
            {
                con.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@flag", flag!);
                parameters.Add("@MenuURL", pageUrl!);
                parameters.Add("@TargetId", targetId!);
                results = await con.QueryAsync<object>(spName, parameters);
                IEnumerable<object> enumerable = ((IEnumerable)results).Cast<object>();
                con.Close();
            }
            return new CustomReponse
            {
                Data = JsonSerializer.Serialize(new
                {
                    Data = results,
                }),
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<int> ExecuteAsync(string Query, IDictionary<string, object> parameters, CommandType commandType, string? SQLConnectionstr)
    {
        try
        {
            int affectedRows = 0;
            using (var db = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await db.OpenAsync();
                affectedRows = parameters == null ? await db.ExecuteAsync(Query, commandType: commandType) : await db.ExecuteAsync(Query, parameters, commandType: commandType);
                await db.CloseAsync();
            }
            return affectedRows;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<string> ExecuteScalarAsync(string Query, IDictionary<string, object> parameters, CommandType commandType, string? SQLConnectionstr)
    {
        try
        {
            string data = string.Empty;

            using (var db = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await db.OpenAsync();
                data = parameters == null ? await db.ExecuteScalarAsync<string>(Query, commandType: commandType) : await db.ExecuteScalarAsync<string>(Query, parameters, commandType: commandType);
                await db.CloseAsync();
            }
            return data;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetDataListAsync<T>(string Query, IDictionary<string, object> parameters, CommandType commandType, string? SQLConnectionstr)
    {

        try
        {
            using (var db = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await db.OpenAsync();
                IEnumerable<T> data = parameters == null ? await db.QueryAsync<T>(Query, commandType: commandType) : await db.QueryAsync<T>(Query, parameters, commandType: commandType);
                await db.CloseAsync();
                return data;

            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<DataSet> GetDataSetAsync(string Query, IDictionary<string, object> parameters, CommandType commandType, string? SQLConnectionstr)
    {

        try
        {
            using (var db = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await db.OpenAsync();
                IDataReader list = parameters == null ? await db.ExecuteReaderAsync(Query, commandType: commandType) : await db.ExecuteReaderAsync(Query, parameters, commandType: commandType);
                DataSet dataset = ConvertDataReaderToDataSet(list);
                await db.CloseAsync();
                return dataset;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataSet ConvertDataReaderToDataSet(IDataReader data)
    {
        DataSet ds = new DataSet();
        int i = 0;
        while (!data.IsClosed)
        {
            ds.Tables.Add("Table" + i.ToString());
            ds.EnforceConstraints = false;
            ds.Tables[i].Load(data);
            i++;
        }
        return ds;
    }


    public async Task<List<T>> GetDataListAsync<T>(string query, object value, CommandType text)
    {
        int maxRetries = 3;
        int retryDelayMs = 1000;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                using (var db = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await db.OpenAsync();
                    if (db.State != ConnectionState.Open)
                    {
                        throw new InvalidOperationException("Failed to open database connection.");
                    }
                    var data = await db.QueryAsync<T>(query, value, commandType: text);
                    return data.ToList();
                }
            }
            catch (SqlException ex) when (ex.InnerException is SocketException && attempt < maxRetries)
            {
                Console.WriteLine($"Retry {attempt}/{maxRetries} for query due to SocketException: {ex.Message}");
                await Task.Delay(retryDelayMs);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to execute query after {attempt} attempts: {ex.Message}", ex);
            }
        }

        throw new Exception($"Failed to execute query after {maxRetries} attempts.");
    }

    #region New Mechanism
    public async Task<PagedResult<T>> GetPagedAsync<T>(
        string storedProcedure,
        PagedRequest request,
        Func<DynamicParameters>? extraParams = null)
    {
        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        var parameters = new DynamicParameters();
        parameters.Add("p_page", request.Page);
        parameters.Add("p_length", request.PageSize);
        parameters.Add("p_sort", request.SortColumn);
        parameters.Add("p_direction", request.SortDirection);
        parameters.Add("p_search", request.Search ?? "");

        // Add any extra parameters
        if (extraParams != null)
        {
            var extraParameters = extraParams();
            foreach (var paramName in extraParameters.ParameterNames)
            {
                parameters.Add(paramName, extraParameters.Get<object>(paramName));
            }
        }

        var result = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

        // Fix: Removed reliance on 'TotalCount' property in 'T' and used a separate query or parameter for total count.
        var total = parameters.Get<int>("p_total_count"); // Assuming the stored procedure sets this output parameter.
        return new PagedResult<T>
        {
            Items = result.ToList(),
            TotalRecord = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<PagedResult<T>> GetPagedAsync<T>(string storedFunctionName, PagedRequest request)
    {
        using var conn = new SqlConnection(_context.Database.GetConnectionString());
        var parameters = new DynamicParameters();
        parameters.Add("p_page", request.Page);
        parameters.Add("p_length", request.PageSize);
        parameters.Add("p_sort", request.SortColumn);
        parameters.Add("p_direction", request.SortDirection);
        parameters.Add("p_search", request.Search ?? "");

        string sql = $"SELECT * FROM {storedFunctionName}(@p_page, @p_length, @p_sort, @p_direction, @p_search)";
        var result = await conn.QueryAsync<T>(sql, parameters);
        var items = result.ToList();

        int total = 0;
        if (items.Count > 0)
        {
            var prop = typeof(T).GetProperty("TotalCount");
            if (prop != null)
            {
                total = Convert.ToInt32(prop.GetValue(items[0]));
            }
        }

        return new PagedResult<T>
        {
            Items = items,
            TotalRecord = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    #endregion



}
