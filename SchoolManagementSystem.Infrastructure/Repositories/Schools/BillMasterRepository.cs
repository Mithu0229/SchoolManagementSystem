using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class BillMasterRepository : GenericRepository<BillMaster>, IBillMasterRepository
{
    private readonly ApplicationDbContext _context;
    public BillMasterRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
