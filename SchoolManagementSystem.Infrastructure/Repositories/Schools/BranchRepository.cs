using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Branches.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class BranchRepository : GenericRepository<Branch>, IBranchRepository
{
    private readonly ApplicationDbContext _context;
    public BranchRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
