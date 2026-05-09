using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.FeeTemplates.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class FeeTemplateRepository : GenericRepository<FeeTemplate>, IFeeTemplateRepository
{
    private readonly ApplicationDbContext _context;
    public FeeTemplateRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
