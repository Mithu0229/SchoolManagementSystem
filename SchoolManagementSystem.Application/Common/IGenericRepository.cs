using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Application.Common;
public interface IGenericRepository<T> where T : class
{
    #region find and get
    IQueryable<T> GetAll(bool disabledTracking = false);
    IQueryable<T> GetAllNoneDeleted(bool disabledTracking = false, bool ignoreTenantGuard = false);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate,
       params Expression<Func<T, object>>[] includeProperties);
    T GetSingleNoneDeleted(Expression<Func<T, bool>> predicate);
    T GetSingle(Expression<Func<T, bool>> predicate);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool disableTracking);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
    T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
    IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate);

    Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate, bool ignoreTenantGuard = false);

    #endregion

    #region Duplicate Checker

    Task<bool> IsDuplicateAsync(
    Expression<Func<T, bool>> predicate,
    bool normalize = false,
    string?[]? normalizeProps = null,
    T? reference = null,
    Func<string, string>? normalizationFunc = null);

    #endregion

    #region count 

    int Count();
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    #endregion


    #region add

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);


    #endregion


    #region update
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entity);
    #endregion

    #region delete
    void Delete(T entity, bool isHardDelete = false);
    Task DeleteAsync(T entity, bool isHardDelete = false);
    void Delete(Expression<Func<T, bool>> predicate, bool isHardDelete = false);
    Task<bool> InstantDelete(T entity, bool isHardDelete = false);
    Task<bool> InstantDeleteWithDeactivate(T entity, bool isHardDelete = false);
    //Task<bool> InstantDelete(Expression<Func<T, bool>> predicate, bool isHardDelete = false);
    Task<bool> InstantDeleteAsync(Expression<Func<T, bool>> predicate, bool isHardDelete = false);
    #endregion

    #region Replace

    Task ReplaceManyAsync<TReplace>(
        Expression<Func<TReplace, bool>> predicate,
        IEnumerable<TReplace> newEntities,
        bool useSoftDelete = true)
        where TReplace : class;

    #endregion


    #region filter
    IQueryable<T> Filter(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include, bool isDisableTracking = false);

    #endregion

}