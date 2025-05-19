using OffersHub.Application.Models;
using OffersHub.Application.Models.Categories;

namespace OffersHub.Application.Services.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryServiceModel>> GetAll(CancellationToken cancellationToken);
        Task<PagedResult<CategoryServiceModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<CategoryServiceModel> Get(string name, CancellationToken cancellationToken);
        Task<CategoryServiceModel> Get(int id, CancellationToken cancellationToken);
        Task<CategoryServiceModel> Create(CategoryServiceModel category, CancellationToken cancellationToken);
        Task<CategoryServiceModel> Update(CategoryServiceModel category, CancellationToken cancellationToken);
        Task Delete(string name, CancellationToken cancellationToken);
    }
}
