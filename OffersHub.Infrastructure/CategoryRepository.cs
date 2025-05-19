using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Models;
using OffersHub.Persistance.Context;
using System.Linq.Expressions;
using System.Threading;

namespace OffersHub.Infrastructure
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(OffersHubContext context) : base(context)
        {
        }

        public async Task<Category?> GetById(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Category?> Get(string name, CancellationToken cancellationToken)
        {
            return await Table
                .Where(x => x.Name == name)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public IQueryable<Category> GetAll()
        {
            return Table.AsNoTracking();
        }

        public async Task<Category> Create(Category category, CancellationToken cancellationToken)
        {
            await base.CreateAsync(category, cancellationToken).ConfigureAwait(false);
            return category;
        }

        public async Task<bool> Exists(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken)
        {
            return await base.AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public new Category Update(Category category)
        {
            base.Update(category);
            return category;
        }

        public new void Attach (Category category)
        {
            base.Attach(category);
        }

        public new void Detach (Category category)
        {
            base.Detach(category);
        }
    }
}
