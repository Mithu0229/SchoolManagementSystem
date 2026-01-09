using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Sitemaps.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories;
public class SitemapRepository : GenericRepository<Sitemap>, ISitemapRepository
{
    private readonly ApplicationDbContext _context;
    public SitemapRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}