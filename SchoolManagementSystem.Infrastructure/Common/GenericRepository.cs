using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.Common.Helpers;
using SchoolManagementSystem.Domain.Common;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Infrastructure.Common;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ICurrentUserService _currentUserService;

    protected Guid? _currentUserId => _currentUserService.UserId ?? Guid.Empty;
    protected Guid? _tenantId => _currentUserService.TenantId ?? Guid.Empty;

    public GenericRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _currentUserService = currentUserService;
    }

    public virtual IQueryable<T> Query(bool disableTracking = false)
    {
        IQueryable<T> query = _dbSet;
        if (disableTracking) query = query.AsNoTracking();

        if (typeof(IGuardTenant).IsAssignableFrom(typeof(T)))
            query = query.Where(e => EF.Property<Guid>(e, "TenantId") == _tenantId);

        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
            query = query.Where(e => !EF.Property<bool>(e, "IsDeleted"));

        return query;
    }

    public IQueryable<T> GetAll(bool disableTracking = false) => Query(disableTracking);

    public IQueryable<T> GetAllNoneDeleted(bool disableTracking = false, bool ignoreTenantGuard = false)
    {
        IQueryable<T> query = _dbSet;
        if (!ignoreTenantGuard && typeof(IGuardTenant).IsAssignableFrom(typeof(T)))
            query = query.Where(e => EF.Property<Guid>(e, "TenantId") == _tenantId);
        query = query.Where(e => !EF.Property<bool>(e, "IsDeleted"));
        return query;
    }

    public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        => Query().Where(predicate);

    public IQueryable<T> Filter(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        bool disableTracking = false)
    {
        IQueryable<T> query = Query(disableTracking);
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        return query;
    }

    public T GetSingle(Expression<Func<T, bool>> predicate) => Query().FirstOrDefault(predicate);

    public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = Query();
        foreach (var include in includeProperties)
            query = query.Include(include);
        return query.FirstOrDefault(predicate);
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate) => await Query().FirstOrDefaultAsync(predicate);

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool disableTracking)
        => await Query(disableTracking).FirstOrDefaultAsync(predicate);

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = Query();
        foreach (var include in includeProperties)
            query = query.Include(include);
        return await query.FirstOrDefaultAsync(predicate);
    }

    public T GetSingleNoneDeleted(Expression<Func<T, bool>> predicate)
        => GetAllNoneDeleted().FirstOrDefault(predicate);

    public async Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate)
        => await GetAllNoneDeleted().FirstOrDefaultAsync(predicate);


    public async Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate, bool ignoreTenantGuard = false)
    => await GetAllNoneDeleted(ignoreTenantGuard: ignoreTenantGuard).FirstOrDefaultAsync(predicate);



    #region Add

    public async Task AddAsync(T entity)
    {
        SetAuditFields(entity);
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
            SetAuditFields(entity);
        await _dbSet.AddRangeAsync(entities);
    }

    #endregion

    #region Update

    public async Task UpdateAsync(T entity)
    {
        SetModifiedFields(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
            SetModifiedFields(entity);
        _dbSet.UpdateRange(entities);
        await Task.CompletedTask;
    }

    #endregion


    public void Delete(T entity, bool hardDelete = false)
    {
        if (entity is ISoftDeletable soft && !hardDelete)
        {
            soft.IsDeleted = true;
            soft.DeletedById = _currentUserId;
            soft.DeletedDate = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }
    }

    public async Task DeleteAsync(T entity, bool hardDelete = false)
    {
        Delete(entity, hardDelete);
        await Task.CompletedTask;
    }

    public void Delete(Expression<Func<T, bool>> predicate, bool hardDelete = false)
    {
        var entities = _dbSet.Where(predicate).ToList();
        foreach (var entity in entities)
            Delete(entity, hardDelete);
    }

    public async Task<bool> InstantDelete(T entity, bool hardDelete = false)
    {
        try
        {
            if (hardDelete)
                await _dbSet.Where(e => e == entity).ExecuteDeleteAsync();
            else
                await _dbSet.Where(e => e == entity).ExecuteUpdateAsync(x => x
                    .SetProperty(p => EF.Property<bool>(p, "IsDeleted"), true)
                    .SetProperty(p => EF.Property<Guid?>(p, "DeletedById"), _currentUserId)
                    .SetProperty(p => EF.Property<DateTime?>(p, "DeletedDate"), DateTime.UtcNow));
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> InstantDeleteWithDeactivate(T entity, bool hardDelete = false)
    {
        try
        {
            if (hardDelete)
                await _dbSet.Where(e => e == entity).ExecuteDeleteAsync();
            else
                await _dbSet.Where(e => e == entity).ExecuteUpdateAsync(x => x
                    .SetProperty(p => EF.Property<bool>(p, "IsDeleted"), true)
                    .SetProperty(p => EF.Property<bool>(p, "IsActive"), false)
                    .SetProperty(p => EF.Property<Guid?>(p, "DeletedById"), _currentUserId)
                    .SetProperty(p => EF.Property<DateTime?>(p, "DeletedDate"), DateTime.UtcNow));
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> InstantDeleteAsync(Expression<Func<T, bool>> predicate, bool hardDelete = false)
    {
        try
        {
            if (hardDelete)
                await _dbSet.Where(predicate).ExecuteDeleteAsync();
            else
                await _dbSet.Where(predicate).ExecuteUpdateAsync(x => x
                    .SetProperty(p => EF.Property<bool>(p, "IsDeleted"), true)
                    .SetProperty(p => EF.Property<bool>(p, "IsActive"), false)
                    .SetProperty(p => EF.Property<Guid?>(p, "DeletedById"), _currentUserId)
                    .SetProperty(p => EF.Property<DateTime?>(p, "DeletedDate"), DateTime.UtcNow));
            return true;
        }
        catch { return false; }
    }

    #region Replace

    public async Task ReplaceManyAsync<TReplace>(
        Expression<Func<TReplace, bool>> predicate,
        IEnumerable<TReplace> newEntities,
        bool useSoftDelete = true)
        where TReplace : class
    {
        var dbSet = _context.Set<TReplace>();
        if (useSoftDelete)
        {
            var entityType = _context.Model.FindEntityType(typeof(TReplace));
            var hasIsActive = entityType?.FindProperty("IsActive") != null;
            await dbSet
                .Where(predicate)
                .ExecuteUpdateAsync(setters =>
                    hasIsActive
                        ? setters
                            .SetProperty(e => EF.Property<bool>(e, "IsDeleted"), true)
                            .SetProperty(e => EF.Property<Guid?>(e, "DeletedById"), _currentUserId)
                            .SetProperty(e => EF.Property<DateTime?>(e, "DeletedDate"), DateTime.UtcNow)
                            .SetProperty(e => EF.Property<bool>(e, "IsActive"), false)
                        : setters
                            .SetProperty(e => EF.Property<bool>(e, "IsDeleted"), true)
                            .SetProperty(e => EF.Property<Guid?>(e, "DeletedById"), _currentUserId)
                            .SetProperty(e => EF.Property<DateTime?>(e, "DeletedDate"), DateTime.UtcNow));
        }
        else
        {
            await dbSet.Where(predicate).ExecuteDeleteAsync();
        }

        foreach (var entity in newEntities)
        {
            SetAuditFields(entity);
        }
        await dbSet.AddRangeAsync(newEntities);
    }


    #endregion



    #region Duplicate-Checker

    public async Task<bool> IsDuplicateAsync(
    Expression<Func<T, bool>> predicate,
    bool normalize = false,
    string?[]? normalizeProps = null,
    T? reference = null,
    Func<string, string>? normalizationFunc = null)
    {
        IQueryable<T> query = _dbSet;

        // Tenant filter
        if (typeof(IGuardTenant).IsAssignableFrom(typeof(T)))
            query = query.Where(e => EF.Property<Guid>(e, "TenantId") == _tenantId);

        query = query.Where(e => !EF.Property<bool>(e, "IsDeleted"));

        // Direct DB query if not normalizing
        if (!normalize || normalizeProps == null || reference == null)
            return await query.AnyAsync(predicate);

        // Load filtered data
        var candidates = await query.ToListAsync();

        var comparer = predicate.Compile();
        return candidates.Any(existing =>
        {
            bool match = normalizeProps.All(prop =>
            {
                var propInfo = typeof(T).GetProperty(prop!);
                if (propInfo == null) return false;

                var val1 = propInfo.GetValue(existing)?.ToString() ?? "";
                var val2 = propInfo.GetValue(reference)?.ToString() ?? "";

                var normalized1 = normalizationFunc?.Invoke(val1) ?? DuplicateCheckHelper.NormalizeString(val1);
                var normalized2 = normalizationFunc?.Invoke(val2) ?? DuplicateCheckHelper.NormalizeString(val2);

                return normalized1 == normalized2;
            });

            return comparer(existing) && match;
        });
    }


    #endregion

    public int Count() => Query().Count();
    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate) => await Query().CountAsync(predicate);

    //private void SetAuditFields(object entity)
    //{
    //    bool isAuditable = entity is IAuditable; // Should be true
    //    bool isTenant = entity is IGuardTenant;  // Should be true
    //    if (entity is IAuditable auditable)
    //    {

    //        auditable.CreatedById = (auditable.CreatedById == Guid.Empty) ? (Guid)_currentUserId! : auditable.CreatedById; //_currentUserId ?? Guid.Empty;
    //        auditable.CreatedDate = DateTime.UtcNow;
    //    }
    //    if (entity is IGuardTenant tenantEntity)
    //    {
    //        tenantEntity.TenantId = (tenantEntity.TenantId == Guid.Empty || tenantEntity.TenantId == null) ? (Guid)_tenantId! : tenantEntity.TenantId; //_currentUserId ?? Guid.Empty;
    //    }
    //}
    private void SetAuditFields(object entity)
    {
        if (entity is IAuditable auditable)
        {
            if (auditable.CreatedById == Guid.Empty)
                auditable.CreatedById = _currentUserId ?? Guid.Empty;
            auditable.CreatedDate = DateTime.UtcNow;
        }

        if (entity is IGuardTenant tenantEntity)
        {
            if ((tenantEntity.TenantId == null || tenantEntity.TenantId == Guid.Empty) && _tenantId != null && _tenantId != Guid.Empty)
            {
                tenantEntity.TenantId = _tenantId.Value;
            }
        }
    }


    private void SetModifiedFields(T entity)
    {
        if (entity is IAuditable auditable)
        {
            auditable.ModifiedById = _currentUserId ?? Guid.Empty;
            auditable.ModifiedDate = DateTime.UtcNow;
        }
    }
}