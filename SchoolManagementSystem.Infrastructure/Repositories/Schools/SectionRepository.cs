using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Sections.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class SectionRepository : GenericRepository<Section>, ISectionRepository
{
    private readonly ApplicationDbContext _context;
    public SectionRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
