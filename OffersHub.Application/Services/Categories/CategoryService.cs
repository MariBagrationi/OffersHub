using Mapster;
using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Exceptions.Categories;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Categories;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;

namespace OffersHub.Application.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CategoryServiceModel> Create(CategoryServiceModel category, CancellationToken cancellationToken)
        {
            bool exist = await _categoryRepository.Exists(x => x.Name == category.Name, cancellationToken).ConfigureAwait(false);
            if (exist)
                throw new CategoryAlreadyExists("Category with such name already exists");

            var entity = category.Adapt<Category>();
            await _categoryRepository.Create(entity, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity.Adapt<CategoryServiceModel>();
        }

        public async Task Delete(string name, CancellationToken cancellationToken)
        {
            bool exist = await _categoryRepository.Exists(x => x.Name == name, cancellationToken).ConfigureAwait(false);
            if (!exist)
                throw new CategoryDoesNotExist("Category with such name does not exist");

            var entity = await _categoryRepository.Get(name, cancellationToken).ConfigureAwait(false);
            _categoryRepository.Delete(entity!);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<CategoryServiceModel> Get(string name, CancellationToken cancellationToken)
        {
            bool exist = await _categoryRepository.Exists(x => x.Name == name, cancellationToken).ConfigureAwait(false);
            if (!exist)
                throw new CategoryDoesNotExist("Category with such name does not exist");

            var result = await _categoryRepository.Get(name, cancellationToken).ConfigureAwait(false);
            return result.Adapt<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> Get(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetById(id, cancellationToken).ConfigureAwait(false);
            return category.Adapt<CategoryServiceModel>();
        }

        public async Task<IEnumerable<CategoryServiceModel>> GetAll(CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAll(cancellationToken).ConfigureAwait(false);
            return categories.Adapt<IEnumerable<CategoryServiceModel>>();
        }

        public async Task<PagedResult<CategoryServiceModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _categoryRepository.GetAll();

            var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            var categories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return new PagedResult<CategoryServiceModel>()
            {
                Items = categories.Adapt<List<CategoryServiceModel>>(),
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };
        }

        public async Task<CategoryServiceModel> Update(CategoryServiceModel category, CancellationToken cancellationToken) 
        {
            bool exist = await _categoryRepository.Exists(x => x.Name == category.Name, cancellationToken).ConfigureAwait(false);
            if (!exist)
                throw new CategoryDoesNotExist("Category you are trying to update with such name does not exist");

            var entity = await _categoryRepository.Get(category.Name, cancellationToken).ConfigureAwait(false);
            _categoryRepository.Delete(entity!);
            var updated = category.Adapt<Category>();
            _categoryRepository.Update(updated!);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return updated.Adapt<CategoryServiceModel>();
        }
    }
}
