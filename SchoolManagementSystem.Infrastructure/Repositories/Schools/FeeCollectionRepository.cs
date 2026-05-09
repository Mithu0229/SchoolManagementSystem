using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.FeeCollections.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class FeeCollectionRepository : GenericRepository<FeeCollection>, IFeeCollectionRepository
{
    private readonly ApplicationDbContext _context;
    public FeeCollectionRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
