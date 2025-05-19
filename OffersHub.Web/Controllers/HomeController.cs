using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Services.Clients;
using OffersHub.Web.Models;
using System.Diagnostics;

namespace OffersHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientService _clientService;

        public HomeController(ILogger<HomeController> logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult CompanyDashboard()
        {
            return View();
        }

        public async Task<IActionResult> ClientDashboard(int id)
        {
            var client = await _clientService.GetById(id, CancellationToken.None);  
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
