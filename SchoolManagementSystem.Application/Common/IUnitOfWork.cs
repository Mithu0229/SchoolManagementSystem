using SchoolManagementSystem.Application.GS.Divisions.Repositories;
using SchoolManagementSystem.Application.GS.Roles.Repositories;
using SchoolManagementSystem.Application.GS.Sitemaps.Repositories;
using SchoolManagementSystem.Application.GS.Tenants.Repository;
using SchoolManagementSystem.Application.GS.Users.Repositories;
using SchoolManagementSystem.Application.School.AcademicClasses.Repositories;
using SchoolManagementSystem.Application.School.AcademicSessions.Repositories;
using SchoolManagementSystem.Application.School.Admissions.Repositories;
using SchoolManagementSystem.Application.School.Branches.Repositories;
using SchoolManagementSystem.Application.School.BillMasters.Repositories;
using SchoolManagementSystem.Application.School.FeeCollections.Repositories;
using SchoolManagementSystem.Application.School.FeeHeads.Repositories;
using SchoolManagementSystem.Application.School.FeeTemplates.Repositories;
using SchoolManagementSystem.Application.School.FinancialYears.Repositories;
using SchoolManagementSystem.Application.School.Institutes.Repositories;
using SchoolManagementSystem.Application.School.SchoolStudents.Repositories;
using SchoolManagementSystem.Application.School.Sections.Repositories;
using SchoolManagementSystem.Application.School.Shifts.Repositories;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Repositories;
using SchoolManagementSystem.Application.School.Students.Repositories;
using SchoolManagementSystem.Application.School.StudentGroups.Repositories;
using SchoolManagementSystem.Application.School.AttendanceDevices.Repositories;
using SchoolManagementSystem.Application.School.Teachers.Repositories;
using SchoolManagementSystem.Application.School.SMSHistories.Repositories;
using SchoolManagementSystem.Application.School.BkashTransactions.Repositories;

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
    public IStudentRepository StudentRepository { get; }
    public IAdmissionRepository AdmissionRepository { get; }
    public IFeeTemplateRepository FeeTemplateRepository { get; }
    public IStudentFeeLedgerRepository StudentFeeLedgerRepository { get; }
    public IFeeCollectionRepository FeeCollectionRepository { get; }
    public IBillMasterRepository BillMasterRepository { get; }
    public IAttendanceDeviceRepository AttendanceDeviceRepository { get; }
    public ITeacherRepository TeacherRepository { get; }
    public ISMSHistoryRepository SMSHistoryRepository { get; }
    public IBkashTransactionRepository BkashTransactionRepository { get; }
    #endregion

    #region Students
    public IStudentInfoRepository StudentInfoRepository { get; }
    
    #endregion
    public IDapperCommandQuery DapperCommandQuery { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
