using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.StudentGroups.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class StudentGroupRepository : GenericRepository<StudentGroup>, IStudentGroupRepository
{
    private readonly ApplicationDbContext _context;
    public StudentGroupRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
