using SchoolManagementSystem.Application.GS.Sitemaps.Models;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Commands;

public class InsertSitemapCommand : IHttpRequest
{
    public SitemapRequest Sitemap { get; set; }

}

