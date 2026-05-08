using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Institutes.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class InstituteRepository : GenericRepository<Institute>, IInstituteRepository
{
    private readonly ApplicationDbContext _context;
    public InstituteRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
