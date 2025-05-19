using OffersHub.Application.Models;
using OffersHub.Application.Models.Companies;

namespace OffersHub.Application.Services.Companies
{
    public interface ICompanyService
    {
        IEnumerable<CompanyResponseModel> GetAll();
        Task<PagedResult<CompanyResponseModel>> GetAllPaged(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<CompanyResponseModel> GetByUserName(string userName, CancellationToken cancellationToken);
        Task<CompanyResponseModel> GetById(int id, CancellationToken cancellationToken);
        Task<CompanyResponseModel> Create(CompanyRequestModel company, CancellationToken cancellationToken);
        Task<CompanyResponseModel> Activate(string userName, CancellationToken cancellationToken);
        Task<CompanyResponseModel> Update(CompanyRequestModel company, CancellationToken cancellationToken);
        Task Delete(string userName, CancellationToken cancellationToken);
    }
}
