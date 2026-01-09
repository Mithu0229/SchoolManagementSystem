using SchoolManagementSystem.Application.GS.Sitemaps.Models;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Commands;

public class UpdateSitemapCommand : IHttpRequest
{
    public SitemapRequest Sitemap { get; set; }

}
