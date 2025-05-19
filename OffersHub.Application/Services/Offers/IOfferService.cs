using OffersHub.Application.Models;
using OffersHub.Application.Models.Offers;
using OffersHub.Domain.Models;
using System.Linq.Expressions;

namespace OffersHub.Application.Services.Offers
{
    public interface IOfferService
    {
        Task<IEnumerable<OfferResponseModel>> GetAll(CancellationToken cancellationToken);
        Task<PagedResult<OfferResponseModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<OfferResponseModel>> GetAllFilered(string? category, string? companyName,
                                                       bool? priceAsc, 
                                                       bool? priceDesc,
                                                       CancellationToken cancellationToken);

        Task<OfferResponseModel> Get(int id, CancellationToken cancellationToken);

        Task<IEnumerable<OfferResponseModel>> GetByPredicate(Expression<Func<Offer, bool>> predicate, CancellationToken cancellationToken);
        Task<OfferResponseModel> Create(OfferRequestModel request, CancellationToken cancellationToken);
        Task<OfferResponseModel> Update(int id, OfferRequestModel request, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);

        // Deactivate offer within - 10 min
        Task<bool> CancelOffer(int id, CancellationToken cancellationToken);

    }
}
