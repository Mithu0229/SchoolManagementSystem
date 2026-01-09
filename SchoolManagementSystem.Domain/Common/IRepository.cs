using System.Linq.Expressions;

namespace SchoolManagementSystem.Domain.Common
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        IQueryable<T> GetAllNoneDeleted(bool disableTracking = false);
        Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate);
        Task<bool> InstantDelete(T entity, bool isHardDelete = false);
        Task AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    }
}
