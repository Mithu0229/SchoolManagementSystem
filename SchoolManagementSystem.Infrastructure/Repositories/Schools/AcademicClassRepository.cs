using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AcademicClasses.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class AcademicClassRepository : GenericRepository<AcademicClass>, IAcademicClassRepository
{
    private readonly ApplicationDbContext _context;
    public AcademicClassRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
