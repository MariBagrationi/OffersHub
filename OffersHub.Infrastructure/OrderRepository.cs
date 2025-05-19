using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Models;
using OffersHub.Persistance.Context;
using System.Linq.Expressions;

namespace OffersHub.Infrastructure
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(OffersHubContext context) : base(context)
        {
        }

        public async Task<Order> Create(Order order, CancellationToken cancellationToken)
        {
            await base.CreateAsync(order, cancellationToken).ConfigureAwait(false);
            return order;
        }

        public async Task<bool> Delete(int orderId, CancellationToken cancellationToken)
        {
            await base.DeleteAsync(new object[] {orderId}, cancellationToken).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> Exists(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken)
        {
            return await base.AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public IQueryable<Order> GetAll()
        {
            return base.Table;
        }

        public async Task<Order?> GetOrderById(int orderId, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { orderId }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
