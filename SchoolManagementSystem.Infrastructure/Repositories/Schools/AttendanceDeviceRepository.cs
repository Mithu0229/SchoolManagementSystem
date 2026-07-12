using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AttendanceDevices.Repositories;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class AttendanceDeviceRepository : GenericRepository<AttendanceDevice>, IAttendanceDeviceRepository
{
    private readonly ApplicationDbContext _context;
    public AttendanceDeviceRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
