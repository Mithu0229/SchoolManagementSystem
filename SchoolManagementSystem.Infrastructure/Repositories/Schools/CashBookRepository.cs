using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.CashBooks.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class CashBookRepository : GenericRepository<CashBook>, ICashBookRepository
{
    private readonly ApplicationDbContext _context;

    public CashBookRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
