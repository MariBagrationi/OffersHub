using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Categories;
using OffersHub.Application.Services.Categories;

namespace OffersHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<PagedResult<CategoryServiceModel>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10, 
            CancellationToken cancellationToken = default)
        {
            return await _categoryService.GetAllPaged(page, pageSize, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("name")]
        public async Task<CategoryServiceModel> GetCategoryById(string name, CancellationToken cancellationToken)
        {
            return await _categoryService.Get(name, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<CategoryServiceModel> Create([FromBody] CategoryServiceModel category,
                                                                CancellationToken cancellationToken)
        {
            return await _categoryService.Create(category, cancellationToken).ConfigureAwait(false);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<CategoryServiceModel> Update([FromBody] CategoryServiceModel category,
                                                                CancellationToken cancellationToken)
        {
            return await _categoryService.Update(category, cancellationToken).ConfigureAwait(false);
        }

        [HttpDelete("name")]
        [Authorize(Roles = "Admin")]
        public async Task Delete(string name, CancellationToken cancellationToken)
        {
            await _categoryService.Delete(name, cancellationToken).ConfigureAwait(false);
        }
    }
}
