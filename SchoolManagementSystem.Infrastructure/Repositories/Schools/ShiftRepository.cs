using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Shifts.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class ShiftRepository : GenericRepository<Shift>, IShiftRepository
{
    private readonly ApplicationDbContext _context;
    public ShiftRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
