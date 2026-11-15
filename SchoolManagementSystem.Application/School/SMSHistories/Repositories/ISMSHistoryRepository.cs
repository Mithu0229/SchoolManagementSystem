using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Application.School.SMSHistories.Repositories;

public interface ISMSHistoryRepository : IGenericRepository<SMSHistory>
{
}

public interface IAttendanceLogRepository : IGenericRepository<AttendanceLog>
{
}


