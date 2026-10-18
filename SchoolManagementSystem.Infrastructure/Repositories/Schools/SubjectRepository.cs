using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Subjects.Repositories;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
{
    private readonly ApplicationDbContext _context;
    public SubjectRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
