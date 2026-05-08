using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.FinancialYears.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class FinancialYearRepository : GenericRepository<FinancialYear>, IFinancialYearRepository
{
    private readonly ApplicationDbContext _context;
    public FinancialYearRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
