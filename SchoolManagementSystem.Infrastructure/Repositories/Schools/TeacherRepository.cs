using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Teachers.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    private readonly ApplicationDbContext _context;
    public TeacherRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
