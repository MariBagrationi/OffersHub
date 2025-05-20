using Mapster;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Repositories;
using OffersHub.Application.Services.Clients;
using OffersHub.Web.Models;
using OffersHub.Web.Models.ViewModels;
using System.Diagnostics;

namespace OffersHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientService _clientService;
        private readonly ICompanyRepository _companyRepository;

        public HomeController(ILogger<HomeController> logger, IClientService clientService, ICompanyRepository companyRepository)
        {
            _logger = logger;
            _clientService = clientService;
            _companyRepository = companyRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }

        public async Task<IActionResult> CompanyDashboard(int id)
        {
            var company = await _companyRepository.GetById(id, CancellationToken.None);
            if (company == null)
            {
                return NotFound(); 
            }
            return View(company.Adapt<CompanyDashboardViewModel>());
        }

        public async Task<IActionResult> ClientDashboard(int id)
        {
            var client = await _clientService.GetById(id, CancellationToken.None);
            if (client == null)
            {
                return NotFound(); 
            }
            return View(client);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
