using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.ExamMarks.Repositories;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class ExamComponentRepository : GenericRepository<ExamComponent>, IExamComponentRepository
{
    private readonly ApplicationDbContext _context;
    public ExamComponentRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
