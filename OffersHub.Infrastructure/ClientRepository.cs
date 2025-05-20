using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Models;
using OffersHub.Persistance.Context;
using System.Linq.Expressions;

namespace OffersHub.Infrastructure
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(OffersHubContext context) : base(context)
        {
        }

        public Task<Client?> Any(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Client> Create(Client client, CancellationToken cancellationToken)
        {
            await base.CreateAsync(client, cancellationToken).ConfigureAwait(false);
            return client;
        }

        public new void Delete(Client client)
        {
            base.Delete(client);
        }
      
        public async Task<bool> Exists(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken)
        {
            return await base.AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Client?> Get(Client client, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] {client.Id}, cancellationToken).ConfigureAwait(false);
        }

        public IQueryable<Client> GetAll()
        {
            return Table.AsNoTracking();
        }

        public async Task<IEnumerable<Client>> GetAll(CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Client?> GetById(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Client?> GetByUserName(string userName, CancellationToken cancellationToken)
        {
            return await base.FindAsync(x => x.UserName == userName, cancellationToken).ConfigureAwait(false);
        }

        public new Client Update(Client client)
        {
            base.Update(client);
            return client;
        }
      

    }
}
