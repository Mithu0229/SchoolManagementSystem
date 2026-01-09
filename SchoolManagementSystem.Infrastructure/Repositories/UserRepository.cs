using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Users.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
