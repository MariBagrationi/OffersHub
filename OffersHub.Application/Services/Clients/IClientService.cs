using OffersHub.Application.Models;
using OffersHub.Application.Models.Clients;
using OffersHub.Application.Models.Offers;

namespace OffersHub.Application.Services.Clients
{
    public interface IClientService
    {
        Task<IEnumerable<ClientServiceModel>> GetAll(CancellationToken cancellationToken);
        Task<PagedResult<ClientServiceModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<ClientServiceModel> GetByUserName(string userName, CancellationToken cancellationToken);
        Task<ClientServiceModel> GetById(int id, CancellationToken cancellationToken);
        Task<ClientServiceModel> Create(ClientServiceModel client, CancellationToken cancellationToken);
        Task<ClientServiceModel> Update(ClientServiceModel client, CancellationToken cancellationToken);
        Task Delete(string userName, CancellationToken cancellationToken);

        Task<bool> AddToCart(int productId, string userName, CancellationToken cancellationToken);
        Task<bool> RemoveFromCart(int productId, string userName, CancellationToken cancellationToken);
        Task<bool> BuyCart(string userName, CancellationToken cancellationToken);
        Task<IEnumerable<OfferResponseModel>> GetCart(int clientid, CancellationToken cancellationToken);

    }
}
