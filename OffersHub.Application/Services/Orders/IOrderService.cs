using OffersHub.Application.Models;
using OffersHub.Application.Models.Orders;

namespace OffersHub.Application.Services.Orders
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderServiceModel>> GetOrders(CancellationToken cancellationToken);
        Task<PagedResult<OrderServiceModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<OrderServiceModel> GetOrderById(int orderId, CancellationToken cancellationToken);
        Task<bool> ChangeOrderStatus(int orderId, string status, CancellationToken cancellationToken); // accesible for company only

        //Deactivate within - 5 min
    }
}
