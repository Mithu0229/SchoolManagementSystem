using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Subjects.Repositories;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class ClassSubjectRepository : GenericRepository<ClassSubject>, IClassSubjectRepository
{
    private readonly ApplicationDbContext _context;
    public ClassSubjectRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
