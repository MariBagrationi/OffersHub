using System.Linq.Expressions;

namespace OffersHub.Domain.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetAsync(object[] key, CancellationToken cancellationToken);
        Task CreateAsync(T entity, CancellationToken cancellationToken);
        //Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(object[] key, CancellationToken cancellationToken);
        void Delete(T entity);
        void Update(T entity);
        //Task DeleteAsync(T entity, CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        IQueryable<T> Table { get; }

    }
}
