using Mapster;
using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Exceptions.Companies;
using OffersHub.Application.Models;
using OffersHub.Application.Models.Companies;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;

namespace OffersHub.Application.Services.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CompanyService(ICompanyRepository companyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CompanyResponseModel> Activate(string userName, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.Get(userName, cancellationToken).ConfigureAwait(false);

            if (company == null)
                throw new CompanyDoesNotExist("User name is incorrect");

            _companyRepository.Attach(company);
            company.IsActive = true;
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return company.Adapt<CompanyResponseModel>();   
        }

        public async Task<CompanyResponseModel> Create(CompanyRequestModel company, CancellationToken cancellationToken)
        {
            var entity = await _companyRepository.Get(company.UserName, cancellationToken).ConfigureAwait(false);
            //bool exists = await _companyRepository.Exists(x => x.User.UserName == company.UserName, cancellationToken).ConfigureAwait(false);
            if (entity != null)
                throw new CompanyAlreadyExist("Company with such user name already exists");

            var exists = await _userRepository.Exists(company.UserName, cancellationToken).ConfigureAwait(false);
            if (!exists)
                throw new CompanyIsNotAuthorized("Before Creating Company Profile, You must Get Registered First");

            var companyDomain = company.Adapt<Company>();
            companyDomain.IsActive = false;
            var user = _userRepository.GetAll().Where(x => x.UserName == company.UserName).FirstOrDefault()!;
            companyDomain.UserId = user.Id;
            companyDomain.User = user;

            if (company.ImageData != null && company.ImageData.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(company.Image);
                var imagePath = Path.Combine("wwwroot", "images", "companies", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
                await File.WriteAllBytesAsync(imagePath, company.ImageData, cancellationToken).ConfigureAwait(false);

                companyDomain.Image = fileName; 
            }
            await _companyRepository.Create(companyDomain, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return companyDomain.Adapt<CompanyResponseModel>();
        }

        public async Task Delete(string userName, CancellationToken cancellationToken)
        {
            bool exist = await _companyRepository.Exists(x => x.User.UserName == userName, cancellationToken).ConfigureAwait(false);
            if(!exist)
                return;

            var company = _companyRepository.GetAll().Where(x => x.User.UserName == userName).FirstOrDefault()!;
            company.IsDeleted = true;
            var user = _userRepository.GetAll().Where(x => x.UserName ==  userName).FirstOrDefault()!;
            user.IsDeleted = true;

            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public IEnumerable<CompanyResponseModel> GetAll()
        {
            var companies = _companyRepository.GetAll()
                                                    .Where(x => !x.IsDeleted)
                                                    .AsEnumerable();

            return companies.Adapt<IEnumerable<CompanyResponseModel>>();
        }

        public async Task<PagedResult<CompanyResponseModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _companyRepository.GetAll();

            var count = query.Count();

            var companies = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return new PagedResult<CompanyResponseModel>
            {
                Items = companies.Adapt<List<CompanyResponseModel>>(),
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };
        }

        public async Task<CompanyResponseModel> GetById(int id, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetById(id, cancellationToken).ConfigureAwait(false);
            return company.Adapt<CompanyResponseModel>();
        }

        public async Task<CompanyResponseModel> GetByUserName(string userName, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.Get(userName, cancellationToken).ConfigureAwait(false);
            if (company == null)
                throw new CompanyDoesNotExist("Company not found for the given user name");

            return company.Adapt<CompanyResponseModel>();
        }

        public async Task<CompanyResponseModel> Update(CompanyRequestModel companyRequest, CancellationToken cancellationToken)
        {
            var companyDomain = await _companyRepository.Get(companyRequest.UserName, cancellationToken).ConfigureAwait(false);
            if (companyDomain == null || companyDomain.IsDeleted)
                throw new CompanyDoesNotExist("Company not found");

            companyDomain.Name = companyRequest.Name;
            companyDomain.Image = companyRequest.Image;

            _companyRepository.Update(companyDomain);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return companyDomain.Adapt<CompanyResponseModel>();
        }
    }
}
