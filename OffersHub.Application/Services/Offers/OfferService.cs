using Mapster;
using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Exceptions.Categories;
using OffersHub.Application.Exceptions.Companies;
using OffersHub.Application.Exceptions.Offers;
using OffersHub.Application.Exceptions.Products;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Offers;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;
using Serilog;
using System.Linq.Expressions;

namespace OffersHub.Application.Services.Offers
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OfferService(IOfferRepository offerRepository, IUnitOfWork unitOfWork, 
                     ICategoryRepository categoryRepository, ICompanyRepository companyRepository)
        {
            _offerRepository = offerRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _companyRepository = companyRepository;
        }
        
        public async Task<IEnumerable<OfferResponseModel>> GetAll(CancellationToken cancellationToken)
        {
            var offers = await _offerRepository.GetAll(cancellationToken).ConfigureAwait(false);
            return offers.Adapt<IEnumerable<OfferResponseModel>>();
        }
        public async Task<OfferResponseModel> Get(int id, CancellationToken cancellationToken)
        {
            var product = await _offerRepository.Get(id, cancellationToken).ConfigureAwait(false);
            return product?.Adapt<OfferResponseModel>()!;
        }

        public async Task<IEnumerable<OfferResponseModel>> GetAllFilered(string? categoryName, string? companyName, bool? priceAsc,
                                                                    bool? priceDesc, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.Get(companyName!, cancellationToken).ConfigureAwait(false);
            if (company == null)
                throw new CompanyDoesNotExist("Company with such user name does not exist");
            var category = await _categoryRepository.Get(categoryName!, cancellationToken).ConfigureAwait(false);
            if (category == null)
                throw new CategoryDoesNotExist("category with such name does not exist");

            int companyId = company.Id;
            int categoryId = category.Id;

            var result = _offerRepository.GetAllFiltered(categoryId, companyId, cancellationToken);

            if (priceDesc != null && priceDesc == true)
                result = result.OrderByDescending(x => x.Price);

            if (priceAsc != null && priceAsc == true)
                result = result.OrderBy(x => x.Price);

            return result.Adapt<IEnumerable<OfferResponseModel>>();
        }
        public async Task<OfferResponseModel> Create(OfferRequestModel offer, CancellationToken cancellationToken)
        {       
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var company = await _companyRepository.Get(offer.Company_UserName, cancellationToken).ConfigureAwait(false);

            if (company == null)
                throw new CompanyDoesNotExist("Cpmpany with such UserName does not exist");

            Log.Information("Company Retrieved: {CompanyId}, UserId: {UserId}", company.Id, company.UserId);

            var category = await _categoryRepository.Get(offer.CategoryName, cancellationToken).ConfigureAwait(false);
            if (category == null)
                throw new CategoryDoesNotExist("Category with such name does not exist");

           
            try
            {
                var entity = offer.Adapt<Offer>();
                entity.CategoryId = category.Id;
                entity.CompanyId = company.Id;

                // Explicitly set the Company navigation property to null to avoid EF trying to insert it
                entity.Company = null;
                entity.Category = null;

                await _offerRepository.Create(entity, cancellationToken).ConfigureAwait(false);   
                await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                transaction.Commit();
                return entity.Adapt<OfferResponseModel>();
            }
            catch(Exception ex) 
            {
                Log.Error(ex, "Error while creating offer: ", ex.InnerException);
                transaction.Rollback();
                throw;
            }
        }

        public async Task<OfferResponseModel> Update(int id, OfferRequestModel offer, CancellationToken cancellationToken)
        {
            var existingProduct = await _offerRepository.Get(id, cancellationToken).ConfigureAwait(false);
            if (existingProduct == null)
                throw new OfferDoesNotExist("Product Does not exist");

            existingProduct = offer.Adapt(existingProduct);
            _offerRepository.Update(existingProduct);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return existingProduct.Adapt<OfferResponseModel>();
        }
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            if (!await _offerRepository.Exists(o => o.Id == id, cancellationToken).ConfigureAwait(false))
                throw new OfferDoesNotExist("Offer with such Id does not exist");

            await _offerRepository.Delete(id, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<OfferResponseModel>> GetByPredicate(Expression<Func<Offer,bool>> predicate, CancellationToken cancellationToken)
        {
            var offers = await _offerRepository.GetByPredicate(predicate, cancellationToken).ConfigureAwait(false);
            return offers.Select(o => o.Adapt<OfferResponseModel>());
        }

        public async Task<bool> CancelOffer(int offerId, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.Get(offerId, cancellationToken).ConfigureAwait(false);

            if (offer!.CreatedAt.AddMinutes(10) <= DateTime.UtcNow)
            {
                return false;
            }

            await _offerRepository.Delete(offerId, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }

        public async Task<PagedResult<OfferResponseModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _offerRepository.GetAll();

            var count = query.Count();

            var offers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return new PagedResult<OfferResponseModel>
            {
                Items = offers.Adapt<List<OfferResponseModel>>(),
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = count
            };
        }
    }
}
