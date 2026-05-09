using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Admissions.Repositories;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class AdmissionRepository : GenericRepository<Admission>, IAdmissionRepository
{
    private readonly ApplicationDbContext _context;
    public AdmissionRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
