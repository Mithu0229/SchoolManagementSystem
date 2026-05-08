using SchoolManagementSystem.Application.GS.Divisions.Repositories;
using SchoolManagementSystem.Application.GS.Roles.Repositories;
using SchoolManagementSystem.Application.GS.Sitemaps.Repositories;
using SchoolManagementSystem.Application.GS.Tenants.Repository;
using SchoolManagementSystem.Application.GS.Users.Repositories;
using SchoolManagementSystem.Application.School.AcademicClasses.Repositories;
using SchoolManagementSystem.Application.School.AcademicSessions.Repositories;
using SchoolManagementSystem.Application.School.Branches.Repositories;
using SchoolManagementSystem.Application.School.FeeHeads.Repositories;
using SchoolManagementSystem.Application.School.FinancialYears.Repositories;
using SchoolManagementSystem.Application.School.Institutes.Repositories;
using SchoolManagementSystem.Application.School.Sections.Repositories;
using SchoolManagementSystem.Application.School.Shifts.Repositories;
using SchoolManagementSystem.Application.School.Students.Repositories;
using SchoolManagementSystem.Application.School.StudentGroups.Repositories;

namespace SchoolManagementSystem.Application.Common;

public interface IUnitOfWork : IDisposable
{

    public ITenantRepository TenantRepository { get; }
    public IUserRoleRepository UserRoleRepository { get; }
    IDivisionRepository DivisionRepository { get; }
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
    ISitemapRepository SitemapRepository { get; }
    public IRoleMenuRepository RoleMenuRepository { get; }
    public IUserLoginHistoryRepository UserLoginHistoryRepository { get; }

    #region School Setup
    public IInstituteRepository InstituteRepository { get; }
    public IBranchRepository BranchRepository { get; }
    public IFinancialYearRepository FinancialYearRepository { get; }
    public IAcademicSessionRepository AcademicSessionRepository { get; }
    public IAcademicClassRepository AcademicClassRepository { get; }
    public ISectionRepository SectionRepository { get; }
    public IShiftRepository ShiftRepository { get; }
    public IStudentGroupRepository StudentGroupRepository { get; }
    public IFeeHeadRepository FeeHeadRepository { get; }
    #endregion

    #region Students
    public IStudentInfoRepository StudentInfoRepository { get; }
    
    #endregion
    public IDapperCommandQuery DapperCommandQuery { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
