using OffersHub.Domain.Models;

namespace OffersHub.Application.Repositories
{
    public interface IClientOfferRepository
    {
        Task<IEnumerable<Offer>> GetCartForClientAsync(int clientId, CancellationToken cancellationToken);
        Task<List<ClientOffer>> GetCartItemsWithOfferAsync(int clientId, CancellationToken cancellationToken);

        IQueryable<ClientOffer> GetClientOffers(int clientId, int offerId);

        ClientOffer? GetClientOffer(int clientId, int offerId);
        Task<ClientOffer?> GetById(int id, CancellationToken cancellationToken);    
        void Attach(ClientOffer clientOffer);
        void Detach(ClientOffer clientOffer);
    }
}
