using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Models.Offers;
using OffersHub.Application.Services.Offers;

namespace OffersHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _productService;

        public OfferController(IOfferService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<OfferResponseModel>> GetAll(string? category, string? companyName,
                                                       bool? priceAsc,
                                                       bool? priceDesc,
                                                       CancellationToken cancellationToken)
        {
            return await _productService.GetAllFilered(category, companyName, priceAsc, priceDesc, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("{id}")]
        public async Task<OfferResponseModel> Get(int id, CancellationToken cancellationToken)
        {
            return await _productService.Get(id, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<OfferResponseModel> Create([FromBody] OfferRequestModel request, CancellationToken cancellationToken)
        {
            return await _productService.Create(request, cancellationToken).ConfigureAwait(false);
        }

        [HttpPut]
        [Authorize(Roles = "Company")]
        public async Task UpdateProduct([FromHeader] int id, [FromBody] OfferRequestModel request, CancellationToken cancellationToken)
        {
            await _productService.Update(id, request, cancellationToken).ConfigureAwait(false);
        }

        [HttpDelete]
        [Authorize(Roles = "Company")]
        public async Task DeleteProduct([FromHeader] int id, CancellationToken cancellationToken)
        {
             await _productService.Delete(id, cancellationToken).ConfigureAwait(false);
           
        }
    }
}
