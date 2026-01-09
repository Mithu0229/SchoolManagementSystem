using SchoolManagementSystem.Application.GS.Divisions.Repositories;
using SchoolManagementSystem.Application.GS.Roles.Repositories;
using SchoolManagementSystem.Application.GS.Sitemaps.Repositories;
using SchoolManagementSystem.Application.GS.Tenants.Repository;
using SchoolManagementSystem.Application.GS.Users.Repositories;
using SchoolManagementSystem.Application.School.Students.Repositories;

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

    #region Students
    public IStudentInfoRepository StudentInfoRepository { get; }
    
    #endregion
    public IDapperCommandQuery DapperCommandQuery { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
