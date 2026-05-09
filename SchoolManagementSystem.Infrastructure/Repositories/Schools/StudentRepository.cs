using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.SchoolStudents.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    private readonly ApplicationDbContext _context;
    public StudentRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
