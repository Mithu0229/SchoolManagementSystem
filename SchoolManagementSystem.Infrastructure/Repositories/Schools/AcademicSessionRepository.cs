using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AcademicSessions.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class AcademicSessionRepository : GenericRepository<AcademicSession>, IAcademicSessionRepository
{
    private readonly ApplicationDbContext _context;
    public AcademicSessionRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
