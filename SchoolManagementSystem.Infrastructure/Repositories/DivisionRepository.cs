using SchoolManagementSystem.Application.GS.Divisions.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Infrastructure.Common;
using SchoolManagementSystem.Infrastructure.Persistence;

namespace SchoolManagementSystem.Infrastructure.Repositories
{
    public class DivisionRepository : Repository<Division>, IDivisionRepository
    {
        public DivisionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
