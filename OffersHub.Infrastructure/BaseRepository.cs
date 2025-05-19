using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using  OffersHub.Domain.Contracts;

namespace OffersHub.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class 
    {
        protected readonly DbContext _context;

        protected readonly DbSet<T> _dbSet;
        public IQueryable<T> Table
        {
            get
            {
                return _dbSet;
            }
        }
        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) =>
            await _dbSet.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);


        public async Task<T?> GetAsync(object[] key, CancellationToken cancellationToken) =>
            await _dbSet.FindAsync(key, cancellationToken).ConfigureAwait(false);

        public async Task CreateAsync(T entity, CancellationToken cancellationToken) =>
            await _dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);

        public async Task DeleteAsync(object[] key, CancellationToken cancellationToken)
        {
            var entity = await _dbSet.FindAsync(key, cancellationToken).ConfigureAwait(false);
            _dbSet.Remove(entity!);
        }

        public void Update(T entity) => _dbSet.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Table.AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }
        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public void Attach(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Attach(entity);
            }
        }

        public void Detach(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }

    
    }
}
