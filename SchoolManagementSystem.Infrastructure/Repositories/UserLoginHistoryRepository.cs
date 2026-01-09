using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Users.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories;
public class UserLoginHistoryRepository : GenericRepository<UsersLoginHistory>, IUserLoginHistoryRepository
{
    private readonly ApplicationDbContext _context;
    public UserLoginHistoryRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }

}
