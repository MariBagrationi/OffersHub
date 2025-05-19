using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;
using System;
using System.Linq.Expressions;

namespace OffersHub.Application.Repositories
{
    public interface ICompanyRepository
    {
        IQueryable<Company> GetAll();
        Task<Company?> GetById(int id, CancellationToken cancellationToken);
        Task<Company?> Get(string userName, CancellationToken cancellationToken);
        Task<Company> Create(Company company, CancellationToken cancellationToken);
        Company Update(Company company);
        Task<bool> Exists(Expression<Func<Company, bool>> predicate, CancellationToken cancellationToken);

        void Attach(Company company);
        void Detach(Company company);
    }
}
