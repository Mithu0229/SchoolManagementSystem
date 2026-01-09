

namespace SchoolManagementSystem.Application.GS.Sitemaps.Queries;

public record GetAllSitemapQuery : IHttpRequest
{
    public PagedRequest MenuPaged { get; set; }
}
