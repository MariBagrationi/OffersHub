using OffersHub.Domain.Models;
using System.Linq.Expressions;

namespace OffersHub.Application.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAll(CancellationToken cancellationToken);
        IQueryable<Client> GetAll();
        Task<Client?> GetById(int id, CancellationToken cancellationToken);
        Task<Client?> Get(Client client, CancellationToken cancellationToken);
        Task<Client> Create(Client client, CancellationToken cancellationToken);
        Client Update(Client client);
        void Delete(Client client);
        Task<bool> Exists(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken);
        Task<Client?> Any(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken);
        Task<Client?> FindAsync(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken);
        void Attach(Client client);
        void Detach(Client client);
    }
}   
