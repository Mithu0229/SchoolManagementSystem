using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BankBooks.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories.Schools;

public class BankBookRepository : GenericRepository<BankBook>, IBankBookRepository
{
    private readonly ApplicationDbContext _context;

    public BankBookRepository(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
}
