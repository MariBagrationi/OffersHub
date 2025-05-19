using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Models.Companies;
using OffersHub.Application.Services.Companies;
using OffersHub.Web.Models.ViewModels;

namespace OffersHub.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("index")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1,
                                                [FromQuery] int pageSize = 10,
                                                CancellationToken cancellationToken = default)
        {
            var companies = await _companyService.GetAllPaged(page, pageSize, cancellationToken).ConfigureAwait(true);
            return View(companies);
        }


        [HttpGet("Details/{username}")]
        public async Task<IActionResult> Details(string username, CancellationToken cancellationToken)
        {
            var company = await _companyService.GetByUserName(username, cancellationToken);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpGet("Create/Client")]
        public IActionResult CreateForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyCreateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await model.ImageFile!.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            await _companyService.Create(model.Adapt<CompanyRequestModel>(), cancellationToken).ConfigureAwait(true);
            return RedirectToAction("Login", "Account");
        }


        [HttpGet("Edit/{username}")]
        public async Task<IActionResult> Edit(string username, CancellationToken cancellationToken)
        {
            var company = await _companyService.GetByUserName(username, cancellationToken);
            if (company == null)
            {
                return NotFound();
            }
            return View(company.Adapt<CompanyEditViewModel>());
        }


        [HttpPost("Edit/{username}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyEditViewModel companyRequest, string username, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(companyRequest);

            await _companyService.Update(companyRequest.Adapt<CompanyRequestModel>(), cancellationToken).ConfigureAwait(true);
            return RedirectToAction("Edit");
        }


        [HttpGet("Delete/{username}")]
        public async Task<IActionResult> Delete(string username, CancellationToken cancellationToken)
        {
            var company = await _companyService.GetByUserName(username, cancellationToken).ConfigureAwait(true);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost("Delete/{username}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string username, CancellationToken cancellationToken)
        {
            await _companyService.Delete(username, cancellationToken).ConfigureAwait(true);
            return RedirectToAction("Delete");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Activate")]
        public async Task<IActionResult> Activate(string username, CancellationToken cancellationToken)
        {
            await _companyService.Activate(username, cancellationToken).ConfigureAwait(true);
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async Task<IActionResult> CompanyDashboard(CancellationToken cancellationToken)
        {
            var userName = User.Identity!.Name;

            var company = await _companyService.GetByUserName(userName!, cancellationToken);
            if (company == null)
                return NotFound(); 

            var model = new CompanyDashboardViewModel
            {
                CompanyId = company.Id,
                CompanyName = company.Name,
                UserName = company.UserName,
                Image = company.Image
            };

            return View(model);
        }

    }
}
