using Mapster;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Exceptions.Categories;
using OffersHub.Application.Models.Categories;
using OffersHub.Application.Services.Categories;
using OffersHub.Application.Services.Offers;
using OffersHub.Web.Models.ViewModels;

namespace OffersHub.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IOfferService _offerService;
        public CategoryController(ICategoryService categoryService, IOfferService offerService)
        {
            _categoryService = categoryService;
            _offerService = offerService;
        }
      
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAll(cancellationToken).ConfigureAwait(true);
            return View(categories);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.Get(id, cancellationToken);
            if (category == null)
                return NotFound();

            var model = category.Adapt<CategoryViewModel>();

            var offers = await _offerService.GetByPredicate(
                o => o.CategoryId == id, cancellationToken);

            model.Offers = offers.Adapt<IEnumerable<OfferViewModel>>();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            string? imageUrl = null;

            if (viewModel.ImageFile != null)
            {
                var fileName = Path.GetFileName(viewModel.ImageFile.FileName);
                var filePath = Path.Combine("wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.ImageFile.CopyToAsync(stream, cancellationToken);
                }

                imageUrl = fileName; 
            }

            var categoryModel = new CategoryServiceModel
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Image = imageUrl
            };

            try
            {
                await _categoryService.Create(categoryModel, cancellationToken);
                return RedirectToAction("GetAll");
            }
            catch (CategoryAlreadyExists ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.Get(id, cancellationToken).ConfigureAwait(true); 
            if (category == null)
                return NotFound();

            return View(category.Adapt<CategoryViewModel>());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string name, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.Delete(name, cancellationToken).ConfigureAwait(false);
                return RedirectToAction("GetAll");
            }
            catch (CategoryDoesNotExist ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("GetAll"); //or show error
            }
        }


    }
}
