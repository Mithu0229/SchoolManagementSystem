using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class StudentFeeLedgerRepository : GenericRepository<StudentFeeLedger>, IStudentFeeLedgerRepository
{
    private readonly ApplicationDbContext _context;
    public StudentFeeLedgerRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
