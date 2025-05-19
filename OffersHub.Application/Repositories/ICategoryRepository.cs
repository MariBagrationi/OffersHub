using OffersHub.Domain.Models;
using System.Linq.Expressions;

namespace OffersHub.Application.Repositories
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetAll();
        Task<Category?> Get(string name, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken);
        Task<Category?> GetById(int id, CancellationToken cancellationToken);
        Task<Category> Create(Category category, CancellationToken cancellationToken);
        Category Update(Category category);
        void Delete(Category category);
        Task<bool> Exists(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken);
        void Attach(Category category);
        void Detach(Category category);
    }
}
