using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Common;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Infrastructure.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetAllNoneDeleted(bool disableTracking = false)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(e => !EF.Property<bool>(e, "IsDeleted"));
            return query;
        }
        public async Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate)
      => await GetAllNoneDeleted().FirstOrDefaultAsync(predicate);

        public async Task<bool> InstantDelete(T entity, bool hardDelete = false)
        {
            try
            {
                if (hardDelete)
                    await _dbSet.Where(e => e == entity).ExecuteDeleteAsync();
                else
                    await _dbSet.Where(e => e == entity).ExecuteUpdateAsync(x => x
                        .SetProperty(p => EF.Property<bool>(p, "IsDeleted"), true)
                        .SetProperty(p => EF.Property<Guid?>(p, "DeletedById"), new Guid())//letter development
                        .SetProperty(p => EF.Property<DateTime?>(p, "DeletedDate"), DateTime.UtcNow));
                return true;
            }
            catch { return false; }
        }
        public virtual async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
                return false;

            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false; // or rethrow, depending on your design
            }
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
                return false;

            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false; // or rethrow, depending on your design
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

    }
}
