using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.SMSHistories.Repositories;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class SMSHistoryRepository : GenericRepository<SMSHistory>, ISMSHistoryRepository
{
    private readonly ApplicationDbContext _context;
    public SMSHistoryRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}

public class AttendanceLogRepository : GenericRepository<AttendanceLog>, IAttendanceLogRepository
{
    private readonly ApplicationDbContext _context;
    public AttendanceLogRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}


