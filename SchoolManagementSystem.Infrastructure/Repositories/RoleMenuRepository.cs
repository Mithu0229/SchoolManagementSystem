using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Roles.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories;
public class RoleMenuRepository : GenericRepository<RoleMenu>, IRoleMenuRepository
{
    private readonly ApplicationDbContext _context;
    public RoleMenuRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}