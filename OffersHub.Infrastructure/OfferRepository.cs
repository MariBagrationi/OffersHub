using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Models.Offers;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Models;
using OffersHub.Persistance.Context;
using System.Linq.Expressions;

namespace OffersHub.Infrastructure
{
    public class OfferRepository : BaseRepository<Offer>, IOfferRepository
    {
        public OfferRepository(OffersHubContext context) : base(context)
        {
        }
        public async Task<Offer?> Get(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public IQueryable<Offer> GetAll()
        {
            return Table;
        }

        public IQueryable<Offer> GetAllFiltered(int? categoryId, int? companyId, CancellationToken cancellationToken)
        {
            var filtered = Table;
            if (categoryId != null)
                filtered = filtered.Where(x => x.CategoryId == categoryId);

            if (companyId != null)
                filtered = filtered.Where(x => x.CompanyId == companyId);

            return filtered;
        }

        public async Task<Offer> Create(Offer offer, CancellationToken cancellationToken)
        {
            await base.CreateAsync(offer, cancellationToken).ConfigureAwait(false);
            return offer;
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await base.DeleteAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> Exists(Expression<Func<Offer, bool>> predicate, CancellationToken cancellationToken)
        {
            return await base.AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public new Offer Update(Offer offer)
        {
            base.Update(offer);
            return offer;
        }
      
        public async Task<List<Offer>> GetExpiredOffersAsync(DateTime currentUtcTime, CancellationToken cancellationToken)
        {
            return await base.Table
                .Where(o => o.OfferDueDate < currentUtcTime && o.Status != OfferStatus.Archived)
                .ToListAsync(cancellationToken);
        }

        public new void Attach(Offer offer)
        {
            base.Attach(offer);
        }

        public new void Detach(Offer offer)
        {
            base.Detach(offer);
        }

        public async Task<IEnumerable<Offer>> GetAll(CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Offer>> GetByPredicate(Expression<Func<Offer, bool>> predicate, CancellationToken cancellationToken)
        {
            return await base.Table
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

    }
}
