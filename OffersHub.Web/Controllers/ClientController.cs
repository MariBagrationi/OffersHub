using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Models.Clients;
using OffersHub.Application.Services.Clients;
using OffersHub.Web.Models.ViewModels;

namespace OffersHub.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var clients = await _clientService.GetAll(cancellationToken);
            return View(clients); 
        }

        [HttpGet]
        public IActionResult CreateClient()
        {
            return View(new ClientCreateViewModel()); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClient(ClientCreateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    var filePath = Path.Combine("wwwroot", "images", model.ImageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream, cancellationToken);
                    }
                }

                var client = new ClientServiceModel
                {
                    UserName = model.UserName,
                    Balance = model.Balance,
                    ImageUrl = model.ImageFile?.FileName 
                };

                var createdClient = await _clientService.Create(client, cancellationToken);

                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction();
        }


        [HttpGet]
        public async Task<IActionResult> UpdateClient(int id, CancellationToken cancellationToken)
        {
            var client = await _clientService.GetById(id, cancellationToken);
            if (client == null)
                return NotFound();

            var model = new ClientServiceModel
            {
                Id = client.Id,
                UserName = client.UserName,
                Balance = client.Balance,
                ImageUrl = client.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClient(ClientServiceModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            //if (model.ImageUrl != null)
            //{
            //    var filePath = Path.Combine("wwwroot", "images", model.ImageUrl);
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await model.ImageUrl.CopyTo(stream, cancellationToken);
            //    }
            //}

            var updatedClient = new ClientServiceModel
            {
                Id = model.Id,
                UserName = model.UserName,
                Balance = model.Balance,
                ImageUrl = model.ImageUrl
            };

            await _clientService.Update(updatedClient, cancellationToken);
            return RedirectToAction("GetAll");
        }



        public async Task<IActionResult> FillBalance(CancellationToken cancellationToken)
        {
            var userName = User.Identity!.Name;
            var client = await _clientService.GetByUserName(userName!, cancellationToken).ConfigureAwait(true);
            if (client == null)
                return NotFound();

            return View(client); 
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FillBalance(int id, ClientServiceModel clientModel, CancellationToken cancellationToken)
        {
            if (id != clientModel.Id)
                return NotFound(); 
            

            if (ModelState.IsValid)
            {
                await _clientService.Update(clientModel, cancellationToken).ConfigureAwait(true);
                return RedirectToAction("GelAll"); 
            }
            return View(clientModel); 
        }

        
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var client = await _clientService.GetById(id, cancellationToken);
            if (client == null)
            {
                return NotFound(); 
            }
            return View(client); 
        }

        [Authorize(Roles = "Client")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var client = await _clientService.GetById(id, cancellationToken);
            if (client == null)
            {
                return NotFound(); 
            }

            await _clientService.Delete(client.UserName, cancellationToken); 
            return RedirectToAction(nameof(Index)); 
        }

        [HttpGet]
        public IActionResult ClientDashboard()
        {
            return View();
        }
    }
}
