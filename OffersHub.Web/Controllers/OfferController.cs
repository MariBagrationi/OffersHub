using Mapster;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Models.Offers;
using OffersHub.Application.Services.Offers;
using OffersHub.Web.Models.ViewModels;

namespace OffersHub.Web.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var offers = await _offerService.GetAll(cancellationToken).ConfigureAwait(true);
            return View(offers);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var offer = await _offerService.Get(id, cancellationToken).ConfigureAwait(true);
            if (offer == null)
                return NotFound();

            return View(offer);
        }

        [HttpGet]
        public IActionResult CreateOffer()
        {
            var userName = User.Identity?.Name;

            var model = new OfferViewModel
            {
                Company_UserName = userName ?? string.Empty 
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOffer(OfferViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _offerService.Create(model.Adapt<OfferRequestModel>(), cancellationToken).ConfigureAwait(true);
            return RedirectToAction("GetAll");
        }

        [HttpGet("EditOffer/{id}")]
        public async Task<IActionResult> EditOffer(int id, CancellationToken cancellationToken)
        {
            var offer = await _offerService.Get(id, cancellationToken).ConfigureAwait(true);
            if (offer == null)
                return NotFound();

            return View(offer.Adapt<OfferViewModel>());
        }

        [HttpPost("EditOffer/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOffer(int id, OfferViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _offerService.Update(id, model.Adapt<OfferRequestModel>(), cancellationToken).ConfigureAwait(true);
            return RedirectToAction("GetAll");
        }

        [HttpGet("DeleteOffer/{id}")]
        public async Task<IActionResult> DeleteOffer(int id, CancellationToken cancellationToken)
        {
            var offer = await _offerService.Get(id, cancellationToken).ConfigureAwait(true);
            if (offer == null)
                return NotFound();

            return View(offer);
        }

        [HttpPost("DeleteOffer/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            await _offerService.Delete(id, cancellationToken).ConfigureAwait(true);
            return RedirectToAction("GetAll");
        }

        [HttpPost]
        public async Task<IActionResult> CancelOffer(int offerId, CancellationToken cancellationToken)
        {
            bool cancelled = await _offerService.CancelOffer(offerId, cancellationToken).ConfigureAwait(true);
            if (cancelled)
                return RedirectToAction("GetAll");

            return RedirectToAction(); 
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOffers(int companyId, CancellationToken cancellationToken)
        {
            var companyOffers = await _offerService.GetByPredicate(o => o.CompanyId == companyId, cancellationToken).ConfigureAwait(true);
            return View(companyOffers);
        }
    }
}
