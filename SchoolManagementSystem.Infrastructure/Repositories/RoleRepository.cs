using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Roles.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories
{
    
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _context = context;
        }
    }
}
