using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Students.Repositories;
using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Infrastructure.Repositories.Students;
public class GuardianInfoRepository : GenericRepository<GuardianInfo>, IGuardianInfoRepository
{
    private readonly ApplicationDbContext _context;
    public GuardianInfoRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}

