using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Common;

public class PagedRequest
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 0;
    public string SortColumn { get; set; } = "Id";
    public string SortDirection { get; set; } = "asc";
    public string? Search { get; set; }
    public List<Filter>? Filters { get; set; } = new List<Filter>();

}

public class Filter
{
    [JsonProperty("field")]
    public string Field { get; set; }

    [JsonProperty("value")]
    public object Value { get; set; }
}

public class PagedResult<T>
{
    public IList<T> Items { get; set; } = new List<T>();
    public int TotalRecord { get; set; }
    //public int FilteredRecord { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

