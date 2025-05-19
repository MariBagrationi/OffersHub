using OffersHub.Domain.Models;
using System.Linq.Expressions;

namespace OffersHub.Application.Repositories
{
    public interface IOfferRepository
    {
        IQueryable<Offer> GetAll();
        Task<IEnumerable<Offer>> GetAll(CancellationToken cancellationToken);
        IQueryable<Offer> GetAllFiltered(int? CategoryId, int? companyId, CancellationToken cancellationToken);
        Task<Offer?> Get(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Offer>> GetByPredicate(Expression<Func<Offer, bool>> predicate, CancellationToken cancellationToken);
        Task<Offer> Create(Offer offer, CancellationToken cancellationToken);
        Offer Update(Offer offer);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<bool> Exists(Expression<Func<Offer, bool>> predicate, CancellationToken cancellationToken);
        Task<List<Offer>> GetExpiredOffersAsync(DateTime dateTime, CancellationToken cancellationToken);
        void Attach(Offer offer);
        void Detach(Offer offer);
    }
}
