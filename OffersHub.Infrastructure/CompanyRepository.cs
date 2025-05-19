using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Models;
using OffersHub.Persistance.Context;
using System.Linq.Expressions;


namespace OffersHub.Infrastructure
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(OffersHubContext context) : base(context)
        {
        }

        public async Task<Company> Create(Company company, CancellationToken cancellationToken)
        {
            await base.CreateAsync(company, cancellationToken).ConfigureAwait(false);
            return company;
        }

        public async Task<bool> Exists(Expression<Func<Company, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Company?> Get(string userName, CancellationToken cancellationToken)
        {
            return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserName == userName && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        }

        public IQueryable<Company> GetAll()
        {
            return _dbSet.AsNoTracking().Include(c => c.User).Where(c => !c.IsDeleted);
        }

        public new Company Update(Company company)
        {
            base.Update(company);
            return company;
        }
       
        public new void Attach(Company company)
        {
            base.Attach(company);
        }

        public new void Detach(Company company)
        {
            base.Detach(company);
        }

        public async Task<Company?> GetById(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

    }
}
