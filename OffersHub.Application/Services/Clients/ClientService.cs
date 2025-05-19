using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Clients;
using OffersHub.Application.Models.Offers;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;

namespace OffersHub.Application.Services.Clients
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientOfferRepository _clientOfferRepository;
        public ClientService(IClientRepository clientRepository, IUnitOfWork unitOfWork, 
                            IUserRepository userRepository, IOrderRepository orderRepository,
                            IOfferRepository offerRepository, IClientOfferRepository clientOfferRepository)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _offerRepository = offerRepository;
            _clientOfferRepository = clientOfferRepository;
        }

        public async Task<ClientServiceModel> Create(ClientServiceModel client, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(client.UserName, cancellationToken);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var entity = client.Adapt<Client>();
            entity.UserId = user.Id; // Map UserId from the retrieved User
            var created = await _clientRepository.Create(entity, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return created.Adapt<ClientServiceModel>();
        }

        public async Task<ClientServiceModel> Update(ClientServiceModel client, CancellationToken cancellationToken)
        {
            var existingClient = await _clientRepository.GetById(client.Id, cancellationToken);
            if (existingClient == null)
                throw new InvalidOperationException("Client not found");

            _clientRepository.Attach(existingClient);
            existingClient.Name = client.Name;
            existingClient.Balance = client.Balance;
            
            var updated = _clientRepository.Update(existingClient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return updated.Adapt<ClientServiceModel>();
        }
        public async Task Delete(string userName, CancellationToken cancellationToken)
        {
            var entity = await _clientRepository.FindAsync(x => x.User.UserName == userName, cancellationToken).ConfigureAwait(false);
            _clientRepository.Delete(entity!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ClientServiceModel>> GetAll(CancellationToken cancellationToken)
        {
            var clients = await _clientRepository.GetAll(cancellationToken).ConfigureAwait(false);
            return clients.Adapt<IEnumerable<ClientServiceModel>>();
        }

        public async Task<ClientServiceModel> GetByUserName(string userName, CancellationToken cancellationToken)
        {
            var entity = await _clientRepository.FindAsync(x => x.User.UserName == userName, cancellationToken).ConfigureAwait(false);
            return entity.Adapt<ClientServiceModel>();
        }

        public async Task<bool> AddToCart(int productId, string userName, CancellationToken cancellationToken)
        {
            var client = await _clientRepository
                .FindAsync(c => c.UserName == userName, cancellationToken);

            if (client == null)
                throw new InvalidOperationException("Client not found");

            var offer = await _offerRepository.Get(productId, cancellationToken).ConfigureAwait(false);
            if (offer == null)
                throw new InvalidOperationException("Offer not found");

            var item = client.Cart.Find(o => o.OfferId == productId && o.ClientId == client.Id);

            if (item != null && !item.IsDeleted) 
            {
                _clientOfferRepository.Attach(item); 
                item.Quantity++;
            }
            else
            {
                client.Cart.Add(new ClientOffer
                {
                    ClientId = client.Id,
                    OfferId = productId,
                    Quantity = 1,
                    DateAdded = DateTime.UtcNow
                });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> BuyCart(string userName, CancellationToken cancellationToken)
        {
            var client = await _clientRepository
                .FindAsync(c => c.User.UserName == userName, cancellationToken);

            if (client == null)
                throw new InvalidOperationException("Client not found");

            var cartItems = await _clientOfferRepository
                .GetCartItemsWithOfferAsync(client.Id, cancellationToken);

            if (!cartItems.Any())
                throw new InvalidOperationException("Cart is empty");

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var totalPrice = cartItems.Sum(ci => ci.Offer.Price * ci.Quantity);

                if (client.Balance < totalPrice)
                    throw new InvalidOperationException("Insufficient balance to complete the purchase.");

                client.Balance -= totalPrice;
                _clientRepository.Attach(client);

                var order = new Order
                {
                    UserName = userName,
                    Status = OrderStatus.Pending,
                    TotalPrice = totalPrice,
                    CreatedAt = DateTime.UtcNow,
                    Items = cartItems.Select(ci => new OrderItem
                    {
                        OfferId = ci.OfferId,
                        Quantity = ci.Quantity
                    }).ToList()
                };

                await _orderRepository.Create(order, cancellationToken);

                foreach (var item in cartItems)
                {
                    _clientOfferRepository.Attach(item);
                    item.IsDeleted = true;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<bool> RemoveFromCart(int productId, string userName, CancellationToken cancellationToken)
        {
            var client = await _clientRepository
                .FindAsync(c => c.User.UserName == userName, cancellationToken);

            if (client == null)
                throw new InvalidOperationException("Client not found");

            var item = _clientOfferRepository.GetClientOffer(client.Id, productId);

            if (item == null)
                return false;

            _clientOfferRepository.Attach(item);
            client.Cart.Remove(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ClientServiceModel> GetById(int id, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetById(id, cancellationToken).ConfigureAwait(false);
            return client.Adapt<ClientServiceModel>();
        }

        public async Task<IEnumerable<OfferResponseModel>> GetCart(int clientId, CancellationToken cancellationToken)
        {
            var offers = await _clientOfferRepository.GetCartForClientAsync(clientId, cancellationToken).ConfigureAwait(false);
            return offers.Adapt<IEnumerable<OfferResponseModel>>();
        }

        public async Task<PagedResult<ClientServiceModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _clientRepository.GetAll();

            var count = query.Count();

            var clients = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return new PagedResult<ClientServiceModel>
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Items = clients.Adapt<List<ClientServiceModel>>(),
                TotalCount = count
            };
        }
    }
}
