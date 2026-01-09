using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Students.Repositories;
using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Infrastructure.Repositories.Students;

public class LocalGuardianInfoRepository : GenericRepository<LocalGuardianInfo>, ILocalGuardianInfoRepository
{
    private readonly ApplicationDbContext _context;
    public LocalGuardianInfoRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
