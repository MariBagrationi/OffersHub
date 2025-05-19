using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Exceptions.Orders;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Orders;
using OffersHub.Application.Services.Orders;

namespace OffersHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet("{id}")]
        public async Task<OrderServiceModel> Get(int id, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrderById(id, cancellationToken).ConfigureAwait(false);
        }


        [HttpGet]
        public async Task<PagedResult<OrderServiceModel>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _orderService.GetAllPaged(page, pageSize, cancellationToken).ConfigureAwait(false);
        }

        
        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Company")]
        public async Task<bool> ChangeOrderStatus(int orderId, [FromBody] string status, CancellationToken cancellationToken)
        {
            return await _orderService.ChangeOrderStatus(orderId, status, cancellationToken).ConfigureAwait(false);
        }
    }
}
