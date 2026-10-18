using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.GradingScales.Queries;

public class GetGradingScaleListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; } = new();
}

public class GetGradingScaleByIdQuery(Guid id) : IHttpRequest
{
    public Guid Id { get; set; } = id;
}

public class GetDefaultGradingScaleQuery : IHttpRequest
{
}
