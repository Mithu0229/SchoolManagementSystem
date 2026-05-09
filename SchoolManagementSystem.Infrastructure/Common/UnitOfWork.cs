using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Divisions.Repositories;
using SchoolManagementSystem.Application.GS.Roles.Repositories;
using SchoolManagementSystem.Application.GS.Sitemaps.Repositories;
using SchoolManagementSystem.Application.GS.Tenants.Repository;
using SchoolManagementSystem.Application.GS.Users.Repositories;
using SchoolManagementSystem.Application.School.AcademicClasses.Repositories;
using SchoolManagementSystem.Application.School.AcademicSessions.Repositories;
using SchoolManagementSystem.Application.School.Admissions.Repositories;
using SchoolManagementSystem.Application.School.Branches.Repositories;
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
using SchoolManagementSystem.Infrastructure.Repositories;
using SchoolManagementSystem.Infrastructure.Repositories.Schools;
using SchoolManagementSystem.Infrastructure.Repositories.Students;

namespace SchoolManagementSystem.Infrastructure.Common
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ICurrentUserService _currentUserService;

        public UnitOfWork(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public IDivisionRepository _divisionRepository;
        public IDivisionRepository DivisionRepository
        {
            get
            {
                if (this._divisionRepository == null)
                {
                    this._divisionRepository = new DivisionRepository(_context);
                }
                return _divisionRepository;
            }
        }

        public IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(_context, _currentUserService);
                }
                return _userRepository;
            }
        }
        public IUserLoginHistoryRepository _usersLoginHistoryRepository;
        public IUserLoginHistoryRepository UserLoginHistoryRepository
        {
            get
            {
                if (this._usersLoginHistoryRepository == null)
                {
                    this._usersLoginHistoryRepository = new UserLoginHistoryRepository(_context, _currentUserService);
                }

                return _usersLoginHistoryRepository;
            }
        }

        public IRoleMenuRepository _roleMenuRepository;
        public IRoleMenuRepository RoleMenuRepository
        {
            get
            {
                if (this._roleMenuRepository == null)
                {
                    this._roleMenuRepository = new RoleMenuRepository(_context, _currentUserService);
                }
                return _roleMenuRepository;
            }
        }
        public ISitemapRepository _sitemapRepository;
        public ISitemapRepository SitemapRepository
        {
            get
            {
                if (this._sitemapRepository == null)
                {
                    this._sitemapRepository = new SitemapRepository(_context, _currentUserService);
                }

                return _sitemapRepository;
            }
        }

        public IDapperCommandQuery _runStoreProcedure;
        public IDapperCommandQuery DapperCommandQuery
        {

            get
            {
                if (this._runStoreProcedure == null)
                {

                    this._runStoreProcedure = new DapperCommandQuery(_context);
                }
                return _runStoreProcedure;
            }
        }

        public IRoleRepository _roleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (this._roleRepository == null)
                {
                    this._roleRepository = new RoleRepository(_context, _currentUserService);
                }
                return _roleRepository;
            }
        }
        public ITenantRepository _tenantRepository;
        public ITenantRepository TenantRepository
        {
            get
            {
                if (this._tenantRepository == null)
                {
                    this._tenantRepository = new TenantRepository(_context, _currentUserService);
                }

                return _tenantRepository;
            }
        }

        public IUserRoleRepository _userRoleRepository;
        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                if (this._userRoleRepository == null)
                {
                    this._userRoleRepository = new UserRoleRepository(_context, _currentUserService);
                }
                return _userRoleRepository;
            }
        }

        #region School Setup

        public IInstituteRepository _instituteRepository;
        public IInstituteRepository InstituteRepository
        {
            get
            {
                if (this._instituteRepository == null)
                {
                    this._instituteRepository = new InstituteRepository(_context, _currentUserService);
                }

                return _instituteRepository;
            }
        }

        public IBranchRepository _branchRepository;
        public IBranchRepository BranchRepository
        {
            get
            {
                if (this._branchRepository == null)
                {
                    this._branchRepository = new BranchRepository(_context, _currentUserService);
                }

                return _branchRepository;
            }
        }

        public IFinancialYearRepository _financialYearRepository;
        public IFinancialYearRepository FinancialYearRepository
        {
            get
            {
                if (this._financialYearRepository == null)
                {
                    this._financialYearRepository = new FinancialYearRepository(_context, _currentUserService);
                }

                return _financialYearRepository;
            }
        }

        public IAcademicSessionRepository _academicSessionRepository;
        public IAcademicSessionRepository AcademicSessionRepository
        {
            get
            {
                if (this._academicSessionRepository == null)
                {
                    this._academicSessionRepository = new AcademicSessionRepository(_context, _currentUserService);
                }

                return _academicSessionRepository;
            }
        }

        public IAcademicClassRepository _academicClassRepository;
        public IAcademicClassRepository AcademicClassRepository
        {
            get
            {
                if (this._academicClassRepository == null)
                {
                    this._academicClassRepository = new AcademicClassRepository(_context, _currentUserService);
                }

                return _academicClassRepository;
            }
        }

        public ISectionRepository _sectionRepository;
        public ISectionRepository SectionRepository
        {
            get
            {
                if (this._sectionRepository == null)
                {
                    this._sectionRepository = new SectionRepository(_context, _currentUserService);
                }

                return _sectionRepository;
            }
        }

        public IShiftRepository _shiftRepository;
        public IShiftRepository ShiftRepository
        {
            get
            {
                if (this._shiftRepository == null)
                {
                    this._shiftRepository = new ShiftRepository(_context, _currentUserService);
                }

                return _shiftRepository;
            }
        }

        public IStudentGroupRepository _studentGroupRepository;
        public IStudentGroupRepository StudentGroupRepository
        {
            get
            {
                if (this._studentGroupRepository == null)
                {
                    this._studentGroupRepository = new StudentGroupRepository(_context, _currentUserService);
                }

                return _studentGroupRepository;
            }
        }

        public IFeeHeadRepository _feeHeadRepository;
        public IFeeHeadRepository FeeHeadRepository
        {
            get
            {
                if (this._feeHeadRepository == null)
                {
                    this._feeHeadRepository = new FeeHeadRepository(_context, _currentUserService);
                }

                return _feeHeadRepository;
            }
        }

        public IStudentRepository _studentRepository;
        public IStudentRepository StudentRepository
        {
            get
            {
                if (this._studentRepository == null)
                {
                    this._studentRepository = new StudentRepository(_context, _currentUserService);
                }

                return _studentRepository;
            }
        }

        public IAdmissionRepository _admissionRepository;
        public IAdmissionRepository AdmissionRepository
        {
            get
            {
                if (this._admissionRepository == null)
                {
                    this._admissionRepository = new AdmissionRepository(_context, _currentUserService);
                }

                return _admissionRepository;
            }
        }

        public IFeeTemplateRepository _feeTemplateRepository;
        public IFeeTemplateRepository FeeTemplateRepository
        {
            get
            {
                if (this._feeTemplateRepository == null)
                {
                    this._feeTemplateRepository = new FeeTemplateRepository(_context, _currentUserService);
                }

                return _feeTemplateRepository;
            }
        }

        public IStudentFeeLedgerRepository _studentFeeLedgerRepository;
        public IStudentFeeLedgerRepository StudentFeeLedgerRepository
        {
            get
            {
                if (this._studentFeeLedgerRepository == null)
                {
                    this._studentFeeLedgerRepository = new StudentFeeLedgerRepository(_context, _currentUserService);
                }

                return _studentFeeLedgerRepository;
            }
        }

        public IFeeCollectionRepository _feeCollectionRepository;
        public IFeeCollectionRepository FeeCollectionRepository
        {
            get
            {
                if (this._feeCollectionRepository == null)
                {
                    this._feeCollectionRepository = new FeeCollectionRepository(_context, _currentUserService);
                }

                return _feeCollectionRepository;
            }
        }

        #endregion

        #region Students

        public IStudentInfoRepository _studentInfoRepository;
        public IStudentInfoRepository StudentInfoRepository
        {
            get
            {
                if (this._studentInfoRepository == null)
                {
                    this._studentInfoRepository = new StudentInfoRepository(_context, _currentUserService);
                }

                return _studentInfoRepository;
            }
        }

        #endregion

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
