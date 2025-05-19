using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Companies;
using OffersHub.Application.Services.Companies;

namespace OffersHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<PagedResult<CompanyResponseModel>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _companyService.GetAllPaged(page, pageSize, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("{userName}")]
        [Authorize(Roles = "Admin")]
        public async Task<CompanyResponseModel?> GetByUserName(string userName, CancellationToken cancellationToken)
        {
            return await _companyService.GetByUserName(userName, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<CompanyResponseModel> CreateCompany([FromBody] CompanyRequestModel request, CancellationToken cancellationToken)
        {
            return await _companyService.Create(request, cancellationToken).ConfigureAwait(false);
        }

        [HttpPatch("{userName}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<CompanyResponseModel> ActivateCompany(string userName, CancellationToken cancellationToken)
        {
            return await _companyService.Activate(userName, cancellationToken).ConfigureAwait(false);
        }

        [HttpPut]
        [Authorize(Roles = "Company")]
        public async Task<CompanyResponseModel> UpdateCompany([FromBody] CompanyRequestModel request, CancellationToken cancellationToken)
        {
            return await _companyService.Update(request, cancellationToken).ConfigureAwait(false);
        }

        [HttpDelete("{userName}")]
        [Authorize(Roles = "Company")]
        public async Task DeleteCompany(string userName, CancellationToken cancellationToken)
        {
            await _companyService.Delete(userName, cancellationToken).ConfigureAwait(false);
        }
    }
}
