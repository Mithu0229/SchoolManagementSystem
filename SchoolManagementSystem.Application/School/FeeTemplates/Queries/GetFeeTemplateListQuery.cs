namespace SchoolManagementSystem.Application.School.FeeTemplates.Queries;

public record GetFeeTemplateListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
