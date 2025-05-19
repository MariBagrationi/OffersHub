using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Services.Clients;
using OffersHub.Application.Services.Offers;
using OffersHub.Application.Services.Orders;
using OffersHub.Domain.Models;

namespace OffersHub.Web.Controllers
{
    [Authorize(Roles = "Client")]
    public class PurchaseController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IOfferService _offerService;
        private readonly IOrderService _orderService;

        public PurchaseController(
            IClientService clientService,
            IOfferService offerService,
            IOrderService orderService)
        {
            _clientService = clientService;
            _offerService = offerService;
            _orderService = orderService;
        }

        [HttpPost("{offerId}")]
        public async Task<IActionResult> AddToCart(int offerId, CancellationToken cancellationToken)
        {
            var userName = User.Identity?.Name!;
            var result = await _clientService.AddToCart(offerId, userName, cancellationToken);

            if (!result)
                return BadRequest("Failed to add item to cart.");

            return RedirectToAction("GoToCart");
        }

        [HttpPost("RemoveFromCart/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId, CancellationToken cancellationToken)
        {
            var userName = User.Identity?.Name!;
            var result = await _clientService.RemoveFromCart(productId, userName, cancellationToken).ConfigureAwait(true);

            if (!result)
                return BadRequest("Failed to remove item from cart.");

            return RedirectToAction("GoToCart");
        }

        [HttpGet]
        public async Task<IActionResult> GoToCart(CancellationToken cancellationToken)
        {
            var userName = User.Identity?.Name!;

            var client = await _clientService.GetByUserName(userName, cancellationToken).ConfigureAwait(true);
            var offers = await _clientService.GetCart(client.Id, cancellationToken).ConfigureAwait(true);
            return View(offers);
        }

        [HttpPost]
        public async Task<IActionResult> BuyCart(CancellationToken cancellationToken)
        {
            var userName = User.Identity?.Name!;
            var result = await _clientService.BuyCart(userName, cancellationToken).ConfigureAwait(true);

            if (!result)
                return BadRequest("Purchase failed.");

            return RedirectToAction("MyOrders");
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetOrders(cancellationToken);
            return View("MyOrders", orders); 
        }

        [HttpGet]
        public async Task<IActionResult> OfferDetails(int id, CancellationToken cancellationToken)
        {
            var offer = await _offerService.Get(id, cancellationToken);
            return View("OfferDetails", offer);
        }
    }
}
