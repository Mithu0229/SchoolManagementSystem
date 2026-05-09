namespace SchoolManagementSystem.Application.School.FeeCollections.Queries;

public record GetFeeCollectionListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; }
}
