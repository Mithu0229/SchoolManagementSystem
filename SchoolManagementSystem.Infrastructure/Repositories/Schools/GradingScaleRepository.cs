using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.GradingScales.Repositories;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class GradingScaleRepository : GenericRepository<GradingScale>, IGradingScaleRepository
{
    private readonly ApplicationDbContext _context;
    public GradingScaleRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
