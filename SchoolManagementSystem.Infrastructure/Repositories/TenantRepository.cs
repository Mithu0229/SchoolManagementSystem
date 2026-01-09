using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Tenants.Repository;

namespace SchoolManagementSystem.Infrastructure.Repositories;
public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
{
    public readonly ApplicationDbContext _context;
    public TenantRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}

