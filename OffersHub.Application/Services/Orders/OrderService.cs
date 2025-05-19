using Mapster;
using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Exceptions.Orders;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Orders;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;

namespace OffersHub.Application.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> ChangeOrderStatus(int orderId, string status, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(orderId, cancellationToken).ConfigureAwait(false);
            if (order == null)
                throw new OrderDoesNotExist("Order with such id, does not exist");

            //check status
            order!.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), status); //tryparse
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<PagedResult<OrderServiceModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _orderRepository.GetAll();

            var count = query.Count();

            var orders = await query
                .Skip((pageNumber - 1)  * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return new PagedResult<OrderServiceModel>
            {
                Items = orders.Adapt<List<OrderServiceModel>>(),
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };
        }

        public async Task<OrderServiceModel> GetOrderById(int orderId, CancellationToken cancellationToken)
        {
            var entity = await _orderRepository.GetOrderById(orderId, cancellationToken).ConfigureAwait(false);
            if (entity == null)
                throw new OrderDoesNotExist("Order with such id does not exists");

            return entity.Adapt<OrderServiceModel?>()!;
        }

        public async Task<IEnumerable<OrderServiceModel>> GetOrders(CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrders(cancellationToken).ConfigureAwait(false);
            return orders.Adapt<IEnumerable<OrderServiceModel>>();
        }
    }
}
