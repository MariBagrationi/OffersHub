using OffersHub.Domain.Models;
using System.Linq.Expressions;

namespace OffersHub.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken);
        IQueryable<Order> GetAll();
        Task<Order?> GetOrderById(int orderId, CancellationToken cancellationToken);
        Task<Order> Create(Order orderServiceModel, CancellationToken cancellationToken);
        Task<bool> Exists(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken);
        Task<bool> Delete(int orderId, CancellationToken cancellation);
        void Attach(Order order);
        void Detach(Order order);
    }
}
