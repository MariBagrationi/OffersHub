using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Clients;
using OffersHub.Application.Services.Clients;

namespace OffersHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<PagedResult<ClientServiceModel>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _clientService.GetAllPaged(page, pageSize, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ClientServiceModel> Get(string userName, CancellationToken cancellationToken)
        {
            return await _clientService.GetByUserName(userName, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<ClientServiceModel> CreateClient([FromBody] ClientServiceModel request, CancellationToken cancellationToken)
        {
            return await _clientService.Create(request, cancellationToken).ConfigureAwait(false);
        }

        [HttpPut]
        [Authorize(Roles = "Client")]
        public async Task<ClientServiceModel> UpdateClient([FromBody] ClientServiceModel request, CancellationToken cancellationToken)
        {
            return await _clientService.Update(request, cancellationToken).ConfigureAwait(false);
        }

        [HttpDelete]
        [Authorize(Roles = "Client")]
        public async Task DeleteClient(string userName, CancellationToken cancellationToken)
        {
            await _clientService.Delete(userName, cancellationToken).ConfigureAwait(false);
        }
    }
}
