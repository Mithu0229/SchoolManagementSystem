using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Divisions.Repositories;
using SchoolManagementSystem.Application.GS.Roles.Repositories;
using SchoolManagementSystem.Application.GS.Sitemaps.Repositories;
using SchoolManagementSystem.Application.GS.Tenants.Repository;
using SchoolManagementSystem.Application.GS.Users.Repositories;
using SchoolManagementSystem.Application.School.Students.Repositories;
using SchoolManagementSystem.Infrastructure.Repositories;
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
