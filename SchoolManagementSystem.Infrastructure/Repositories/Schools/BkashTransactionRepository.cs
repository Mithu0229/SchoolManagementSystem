using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BkashTransactions.Repositories;
using SchoolManagementSystem.Domain.Entities.Schools;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class BkashTransactionRepository : GenericRepository<BkashTransaction>, IBkashTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public BkashTransactionRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
