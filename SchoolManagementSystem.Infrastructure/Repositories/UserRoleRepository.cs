using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Users.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories;
public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    private readonly ApplicationDbContext _context;
    public UserRoleRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
