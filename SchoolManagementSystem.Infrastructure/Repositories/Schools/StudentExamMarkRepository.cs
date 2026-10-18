using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.ExamMarks.Repositories;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class StudentExamMarkRepository : GenericRepository<StudentExamMark>, IStudentExamMarkRepository
{
    private readonly ApplicationDbContext _context;
    public StudentExamMarkRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
