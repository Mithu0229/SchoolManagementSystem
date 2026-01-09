using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Students.Repositories;
using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Infrastructure.Repositories.Students;
public class StudentInfoRepository : GenericRepository<StudentInfo>, IStudentInfoRepository
{
    private readonly ApplicationDbContext _context;
    public StudentInfoRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
