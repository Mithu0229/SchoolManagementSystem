using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.FeeHeads.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class FeeHeadRepository : GenericRepository<FeeHead>, IFeeHeadRepository
{
    private readonly ApplicationDbContext _context;
    public FeeHeadRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
